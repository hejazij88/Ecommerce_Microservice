namespace Basket.Api.Entity;

public class ShoppingCart
{
    public string UserName { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = new();
    public ShoppingCart() { }
    public ShoppingCart(string userName) => UserName = userName;
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}