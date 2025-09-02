using Catalog.Domain.Entity;

namespace Catalog.Applications.Repository;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task CreateAsync(Product product);
    Task UpdateAsync(string id, Product product);
    Task DeleteAsync(string id);
}