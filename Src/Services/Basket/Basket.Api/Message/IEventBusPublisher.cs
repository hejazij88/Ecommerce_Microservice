namespace Basket.Api.Message;

public interface IEventBusPublisher
{
     void Publish<T>(T @event, string? routingKey = null);
}