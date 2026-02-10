using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using NET_API_with_Redis.Middleware;
using NET_API_with_Redis.Repositories;
using NET_API_with_Redis.Service;
using NET_API_with_Redis.Services;
using NET_API_with_Redis.Versioning;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Versioning + versioned API explorer
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerVersioningConfig>();

// Redis
var redisConnection = builder.Configuration["Redis:ConnectionString"];
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

// Sample redis test start 
var redis = ConnectionMultiplexer.Connect(redisConnection);

var db = redis.GetDatabase();
await db.StringSetAsync("name", "Prashant");
var value = await db.StringGetAsync("name");
// Sample redis test end

// Add Services
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<RedisCacheService>();

var app = builder.Build();

// Middleware
app.UseMiddleware<RedisRateLimitMiddleware>();


var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant()
        );
    }
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
