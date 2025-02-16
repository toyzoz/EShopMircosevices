namespace Catalog.API.Products.UpdateProduct;

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .WithDescription("Updates product")
            .WithSummary("Updates product")
            .Produces<UpdateProductResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

public record UpdateProductRequest(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string ImageFile,
    List<string> Category);

public record UpdateProductResponse(bool IsSuccess);