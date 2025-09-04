using Basket.Api.Entity;
using System.Threading.Tasks;

namespace Basket.Api.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart?> GetBasket(string userName);
    Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
    Task DeleteBasket(string userName);
}