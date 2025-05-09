using DemoCQRSApp.Application.Responses.Products;
using DemoCQRSApp.Application.Services;
using FastEndpoints;

namespace DemoCQRSApp.Api.Endpoints.Products;

public class DeleteProductEndpoint : EndpointWithoutRequest<DeleteProductResponse>
{
    private readonly ProductService _service;

    public DeleteProductEndpoint(ProductService service)
    {
        _service = service;
    }

    public override void Configure()
    {
        Delete("/products/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var success = await _service.DeleteProductAsync(id);
        await SendAsync(new DeleteProductResponse { Success = success }, success ? 200 : 404);
    }
}