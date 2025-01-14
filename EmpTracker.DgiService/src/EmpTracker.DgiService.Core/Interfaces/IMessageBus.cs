namespace EmpTracker.DgiService.Core.Interfaces
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, string exchange, string exchangeType, string routingKey) where T : class;
        Task SubscribeAsync<T>(Func<T, Task> handler, string exchange, string exchangeType, string routingKey, string queue) where T : class;

        void DisposeConnection();
    }
}
