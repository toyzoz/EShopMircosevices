using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/order", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();
                var result = await sender.Send(command);
                var res = result.Adapt<CreateOrderResponse>();

                return Results.Created($"/order/{res.Id}", res);
            })
            .WithName("create order").WithDescription("create order").WithSummary("create order")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record CreateOrderRequest(OrderDto Order);

public record CreateOrderResponse(Guid Id);