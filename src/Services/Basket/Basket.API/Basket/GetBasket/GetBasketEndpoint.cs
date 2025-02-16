namespace Basket.API.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{username}",
                async (string username, ISender sender) =>
                {
                    var result = await sender.Send(new GetBasketQuery(username));
                    var response = result.Adapt<GetBasketResponse>();

                    return Results.Ok(response);
                })
            .WithName("GetBasket").WithDescription("Get the basket")
            .WithSummary("Get the basket")
            .Produces<GetBasketResponse>().ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record GetBasketRequest(string Username);

public record GetBasketResponse(ShoppingCart Basket);