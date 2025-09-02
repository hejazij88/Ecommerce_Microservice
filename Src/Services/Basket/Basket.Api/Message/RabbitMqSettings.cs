namespace Basket.Api.Message;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "event_bus"; 
    public string ExchangeType { get; set; } = "topic"; 
    public bool DevBindQueue { get; set; } = true; 
    public string DevQueueName { get; set; } = "dev_basket_checkout_q";
    public string RoutingKey { get; set; } = "basket.checkout";
}