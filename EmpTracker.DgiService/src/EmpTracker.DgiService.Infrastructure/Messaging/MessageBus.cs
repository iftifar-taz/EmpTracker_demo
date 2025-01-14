using System.Net.Mime;
using System.Text;
using System.Text.Json;
using EmpTracker.DgiService.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EmpTracker.DgiService.Infrastructure.Messaging
{
    public class MessageBus : IMessageBus, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private string _consumerTag = string.Empty;

        public MessageBus(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri(configuration.GetSection("RabbitMq").GetSection("Uri").Value!),
                ClientProvidedName = "EmpTraker"
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        }

        public async Task PublishAsync<T>(T message, string exchange, string exchangeType, string routingKey) where T : class
        {
            await _channel.ExchangeDeclareAsync(exchange, exchangeType);
            await _channel.BasicPublishAsync(
                exchange,
                routingKey,
                true,
                new BasicProperties
                {
                    ContentType = MediaTypeNames.Application.Json,
                    DeliveryMode = DeliveryModes.Persistent
                },
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message))
            );
        }

        public async Task SubscribeAsync<T>(Func<T, Task> handler, string exchange, string exchangeType, string routingKey, string queue) where T : class
        {
            await _channel.ExchangeDeclareAsync(exchange, exchangeType);
            await _channel.QueueDeclareAsync(queue, false, false, false, null);
            await _channel.QueueBindAsync(queue, exchange, routingKey, null);
            await _channel.BasicQosAsync(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var obj = JsonSerializer.Deserialize<T>(body);
                if (obj != null)
                {
                    await handler(obj);
                }

                await _channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            };

            _consumerTag = await _channel.BasicConsumeAsync(queue, autoAck: false, consumer);
        }

        public void DisposeConnection()
        {
            Dispose();
        }

        public void Dispose()
        {
            _channel.BasicCancelAsync(_consumerTag).GetAwaiter().GetResult();
            _channel.CloseAsync().GetAwaiter().GetResult();
            _connection.CloseAsync().GetAwaiter().GetResult();

            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
