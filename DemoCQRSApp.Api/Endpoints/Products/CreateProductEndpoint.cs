using DemoCQRSApp.Application.Requests.Products;
using DemoCQRSApp.Application.Responses.Products;
using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products;

public class CreateProductEndpoint : Endpoint<CreateProductRequest, CreateProductResponse>
{
    private readonly ProductService _service;

    public CreateProductEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/products");
    }

    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var id = await _service.CreateProductAsync(req.Name);
        await SendAsync(new CreateProductResponse { Id = id });
    }
}