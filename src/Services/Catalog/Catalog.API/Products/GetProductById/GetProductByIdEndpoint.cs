namespace Catalog.API.Products.GetProductById;

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}",
                async (Guid id, ISender sender) =>
                {
                    var result = await sender.Send(new GetProductByIdQuery(id));
                    var response = result.Adapt<GetProductByIdResponse>();
                    return Results.Ok(response);
                })
            .WithName("GetProductById")
            .WithDescription("Get product by id")
            .WithSummary("Get product by id")
            .Produces<Product>()
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

public record GetProductByIdResponse(Product Product);