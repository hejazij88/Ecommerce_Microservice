using MassTransit;
using Ordering.Api.Data;
using Ordering.Api.Entity;
using Ordering.Api.Events;

namespace Ordering.Api.Consumers;

public class BasketCheckoutConsumer: IConsumer<BasketCheckoutEvent>
{
    private readonly OrderContext _context;

    public BasketCheckoutConsumer(OrderContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var message = context.Message;

        var order = new Order
        {
            UserName = message.UserName,
            FirstName = message.FirstName,
            LastName = message.LastName,
            EmailAddress = message.EmailAddress,
            AddressLine = message.AddressLine,
            Country = message.Country,
            State = message.State,
            ZipCode = message.ZipCode,
            TotalPrice = message.TotalPrice
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }
}