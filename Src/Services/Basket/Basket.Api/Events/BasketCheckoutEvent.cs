namespace Basket.Api.Events
{
    public class BasketCheckoutEvent
    {
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public string EventType { get; set; } = "BasketCheckedOut";
        public string OccurredAt { get; set; } = DateTime.UtcNow.ToString("O");
        public string Version { get; set; } = "1";
        public string CorrelationId { get; set; } = Guid.NewGuid().ToString();


        public string UserName { get; set; } = default!;
        public decimal TotalPrice { get; set; }


        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
    }
}
