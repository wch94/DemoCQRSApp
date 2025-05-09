using DemoCQRSApp.Domain.Entities;

namespace DemoCQRSApp.Domain.Interfaces;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<Product> GetByIdAsync(int id);
}