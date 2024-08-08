using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var redis_connection = Environment.GetEnvironmentVariable("REDIS_CONNECTION") ?? "localhost";

services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redis_connection;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var scope = app.Services.CreateAsyncScope();
var _cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();


app.MapGet("/", () => {
    string redis_key = configuration["Redis:Key"];

    var ultimo_status = _cache.GetString(redis_key);
    var status_atual = $"{DateTime.UtcNow:o}";

    _cache.SetString(redis_key, status_atual);

    return Results.Ok($"Ultimo Status: {ultimo_status} | Status Atual: {status_atual}");
});

app.Run();

