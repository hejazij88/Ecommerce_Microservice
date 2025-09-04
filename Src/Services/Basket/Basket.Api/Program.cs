using Basket.Api.Message;
using Basket.Api.Repositories;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
});

// Redis connection
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});

// Health check For Redis
builder.Services.AddHealthChecks()
    .AddRedis(
        builder.Configuration.GetValue<string>("CacheSettings:ConnectionString"),
        "Redis Health",
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);



autoBind:
var settings = new RabbitMqSettings
{
    HostName = builder.Configuration["RabbitMQ:HostName"] ?? "localhost",
    Port = int.TryParse(builder.Configuration["RabbitMQ:Port"], out var p) ? p : 5672,
    UserName = builder.Configuration["RabbitMQ:UserName"] ?? "guest",
    Password = builder.Configuration["RabbitMQ:Password"] ?? "guest",
    ExchangeName = builder.Configuration["RabbitMQ:ExchangeName"] ?? "event_bus",
    ExchangeType = builder.Configuration["RabbitMQ:ExchangeType"] ?? "topic",
    DevBindQueue = bool.TryParse(builder.Configuration["RabbitMQ:DevBindQueue"], out var dev) ? dev : true,
    DevQueueName = builder.Configuration["RabbitMQ:DevQueueName"] ?? "dev_basket_checkout_q",
    RoutingKey = builder.Configuration["RabbitMQ:RoutingKey"] ?? "basket.checkout"
};


builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<IEventBusPublisher, EventBusPublisher>();


builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
    c.RoutePrefix = string.Empty;
});
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");


app.Run();
