using System.Text.Json;
using RabbitMQ.Client;

namespace Basket.Api.Message;

public class EventBusPublisher:IEventBusPublisher
{
    private readonly RabbitMqSettings _settings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public EventBusPublisher(RabbitMqSettings settings)
    {
        _settings = settings;
        var factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password,
            DispatchConsumersAsync = true
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();


        _channel.ExchangeDeclare(exchange: _settings.ExchangeName,
            type: _settings.ExchangeType,
            durable: true,
            autoDelete: false);


        if (_settings.DevBindQueue)
        {
            _channel.QueueDeclare(queue: _settings.DevQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false);
            _channel.QueueBind(queue: _settings.DevQueueName,
                exchange: _settings.ExchangeName,
                routingKey: _settings.RoutingKey);
        }
    }
    public void Publish<T>(T @event, string? routingKey = null)
    {
        var rk = routingKey ?? _settings.RoutingKey;
        var body = JsonSerializer.SerializeToUtf8Bytes(@event, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        });


        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2; // persistent
        props.MessageId = Guid.NewGuid().ToString();
        props.Type = typeof(T).Name;
        props.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());


        _channel.BasicPublish(exchange: _settings.ExchangeName,
            routingKey: rk,
            mandatory: false,
            basicProperties: props,
            body: body);
    }
    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
    }
}