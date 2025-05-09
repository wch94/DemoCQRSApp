using Application.Requests.Products;
using Application.Responses.Products;
using Application.Services;
using FastEndpoints;

namespace Api.Endpoints.Products;

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