using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Domain.Entities;
using DemoCQRSApp.Domain.Interfaces;

namespace DemoCQRSApp.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> CreateProduct(string name)
    {
        var product = new Product { Name = name };
        await _repository.AddAsync(product);
        return product.Id;
    }

    public async Task<ProductDto> GetProductById(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return new ProductDto { Id = product.Id, Name = product.Name };
    }
}