using DemoCQRSApp.Domain.Entities;
using DemoCQRSApp.Domain.Interfaces;

namespace DemoCQRSApp.Infrastructure.Persistence;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product == null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public async Task AddAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }
}