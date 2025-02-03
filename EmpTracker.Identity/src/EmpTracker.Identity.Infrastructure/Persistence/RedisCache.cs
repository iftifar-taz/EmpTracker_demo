using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public class RedisCache(IConfiguration configuration)
    {
        private readonly IDatabase _database = ConnectionMultiplexer.Connect(configuration.GetSection("Redis").GetSection("ConnectionString").Value!).GetDatabase();

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, expiry);
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
