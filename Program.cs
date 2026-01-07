using Microsoft.AspNetCore.Mvc.ApiExplorer;
using NET_API_with_Redis.Middleware;
using NET_API_with_Redis.Repositories;
using NET_API_with_Redis.Service;
using NET_API_with_Redis.Services;
using NET_API_with_Redis.Versioning;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Api Versioning
builder.Services.AddApiVersioningConfig();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<SwaggerVersioningConfig>();

// Redis
var redisConn = builder.Configuration["REDIS_CONNECTION"] ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConn));

// Sample redis test start 
var redis = ConnectionMultiplexer.Connect("localhost:6379");
var db = redis.GetDatabase();
await db.StringSetAsync("name", "Prashant");
var value = await db.StringGetAsync("name");
// Sample redis test end

// Add Services
builder.Services.AddSingleton<IProductRepository, MockProductRepository>();
builder.Services.AddScoped<ProductService>();


var app = builder.Build();

// Middleware
app.UseMiddleware<RedisRateLimitMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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
