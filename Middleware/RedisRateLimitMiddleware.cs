using StackExchange.Redis;

namespace NET_API_with_Redis.Middleware
{
    public class RedisRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _limit = 3;
        private readonly int _windowSeconds = 100;

        public RedisRateLimitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IConnectionMultiplexer redis)
        {
            var db = redis.GetDatabase();
            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var key = $"ratelimit:{ip}:{DateTime.UtcNow:yyyyMMddHHmm}";
            var count = (int)await db.StringIncrementAsync(key);

            if (count == 1)
                await db.KeyExpireAsync(key, TimeSpan.FromSeconds(_windowSeconds));

            if (count > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "Rate limit exceeded",
                    limit = _limit,
                    window = _windowSeconds
                });
                return;
            }

            await _next(context);
        }
    }
}


