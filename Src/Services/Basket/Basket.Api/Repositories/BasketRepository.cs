using Basket.Api.Entity;

namespace Basket.Api.Repositories;

public class BasketRepository:IBasketRepository
{

    private static readonly Dictionary<string, ShoppingCart> _storage = new();
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        if (!_storage.TryGetValue(userName, out var basket))
            basket = new ShoppingCart(userName);

        return await Task.FromResult(basket);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
    {
        _storage[basket.UserName] = basket;
        return await Task.FromResult(basket);
    }

    public async Task DeleteBasket(string userName)
    {
        _storage.Remove(userName);
    }
}