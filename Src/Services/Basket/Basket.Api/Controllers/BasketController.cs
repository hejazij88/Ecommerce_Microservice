using Basket.Api.Entity;
using Basket.Api.Events;
using Basket.Api.Message;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IEventBusPublisher _publisher;


        public BasketController(IBasketRepository repository, IEventBusPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }


        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
            => Ok(await _repository.GetBasket(userName));


        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
            => Ok(await _repository.UpdateBasket(basket));


        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }


        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutEvent checkout)
        {
            var basket = await _repository.GetBasket(checkout.UserName);
            if (basket.Items.Count == 0)
                return BadRequest("Basket is empty");


            checkout.TotalPrice = basket.TotalPrice;


            _publisher.Publish(checkout, routingKey: "basket.checkout");


            await _repository.DeleteBasket(checkout.UserName);
            return Accepted(new { message = "Checkout accepted", checkout.EventId, checkout.CorrelationId, checkout.TotalPrice });
        }
    }
}
