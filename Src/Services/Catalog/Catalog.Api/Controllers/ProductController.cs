using Catalog.Applications.Repository;
using Catalog.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Catalog.Api.Dtos.Product_DTO;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            var dtos = items.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Stock));
            return Ok(dtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(new ProductDto(item.Id, item.Name, item.Description, item.Price, item.Stock));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock
            };
            await _repo.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateProductDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.Stock = dto.Stock;
            await _repo.UpdateAsync(id, existing);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return NotFound();
            await _repo.DeleteAsync(id);
            return NoContent();
        }

    }
}
