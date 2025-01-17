using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace EmpTracker.Identity.Infrastructure.Persistence
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly StackExchange.Redis.IDatabase _database;

        public RedisCacheService(IConfiguration configuration)
        {
            var muxer = ConnectionMultiplexer.Connect("localhost:6379");
            _redisConnection = muxer;
            _database = muxer.GetDatabase(); ;
        }

        // Get cache value from Redis
        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }

            return value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default;
        }

        // Set cache value to Redis
        public async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, serializedValue, expiry);
        }

        // Remove cache key
        public async Task RemoveCacheAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
}
