using Catalog.Domain.Entity;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Applications.Repository;

public class ProductRepository:IProductRepository
{
    private readonly MongoContext _context;
    public ProductRepository(MongoContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.Find(_ => true).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
        return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        await _context.Products.InsertOneAsync(product);
    }

    public async Task UpdateAsync(string id, Product product)
    {
        await _context.Products.ReplaceOneAsync(p => p.Id == id, product);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Products.DeleteOneAsync(p => p.Id == id);
    }
}