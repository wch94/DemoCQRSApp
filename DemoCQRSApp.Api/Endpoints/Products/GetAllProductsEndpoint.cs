using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products;

public class GetAllProductsEndpoint : EndpointWithoutRequest<IEnumerable<ProductDto>>
{
    private readonly ProductService _service;

    public GetAllProductsEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/products");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var products = await _service.GetAllProductsAsync();
        await SendAsync(products);
    }
}