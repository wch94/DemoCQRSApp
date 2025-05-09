using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products.CreateProduct;

public class CreateProductEndpoint : Endpoint<CreateProductRequest, CreateProductResponse>
{
    private readonly ProductService _service;

    public CreateProductEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/v1/api/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var id = await _service.CreateProduct(req.Name);
        await SendAsync(new CreateProductResponse { Id = id });
    }
}