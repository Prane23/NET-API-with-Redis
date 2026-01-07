using StackExchange.Redis;
using System.Text.Json;

namespace NET_API_with_Redis.Services
{
    public class RedisCacheService
    {
        private readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            var json = value.ToString();
            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetAsync<T>(string key, T data, TimeSpan expiration)
        {
            var json = JsonSerializer.Serialize(data);
            await _db.StringSetAsync(key, json, expiration);
        }
    }
}
