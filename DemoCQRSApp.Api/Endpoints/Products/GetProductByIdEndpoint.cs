using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products;

public class GetProductByIdEndpoint : EndpointWithoutRequest<ProductDto>
{
    private readonly ProductService _service;

    public GetProductByIdEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/products/{Id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var product = await _service.GetProductByIdAsync(id);

        if (product == null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendAsync(product);
    }
}