using Azure;
using Application.Requests.Products;
using Application.Responses.Products;
using Application.Services;
using FastEndpoints;

namespace Api.Endpoints.Products;

public class UpdateProductEndpoint : Endpoint<UpdateProductRequest>
{
    private readonly ProductService _service;

    public UpdateProductEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Put("/products/{id}");
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var id = Route<int>("id");
        var success = await _service.UpdateProductAsync(id, req.Name);
        await SendAsync(new UpdateProductResponse { Success = success }, success ? 200 : 404);
    }
}