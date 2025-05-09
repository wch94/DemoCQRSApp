using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products.GetProductById;

public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, ProductDto>
{
    private readonly ProductService _service;

    public GetProductByIdEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Get("/v1/api/products/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        var product = await _service.GetProductById(req.Id);
        await SendAsync(product);
    }
}