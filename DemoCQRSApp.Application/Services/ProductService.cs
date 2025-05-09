using AutoMapper;
using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Domain.Entities;
using DemoCQRSApp.Domain.Interfaces;

namespace DemoCQRSApp.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<int> CreateProductAsync(string name)
    {
        var product = new Product { Name = name };
        await _repository.AddAsync(product);
        return product.Id;
    }

    public async Task<bool> UpdateProductAsync(int id, string name)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return false;

        product.Name = name;
        await _repository.UpdateAsync(product);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null) return false;

        await _repository.DeleteAsync(product);
        return true;
    }
}