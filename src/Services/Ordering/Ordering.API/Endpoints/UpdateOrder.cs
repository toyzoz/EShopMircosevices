using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateOrderCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateOrderResponse>();

                return Results.Ok(response);
            }).WithName("UpdateOrder").WithDescription("Updates order")
            .WithSummary("Updates order")
            .Produces<UpdateOrderResponse>().ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record UpdateOrderRequest(OrderDto Order);

public record UpdateOrderResponse(bool IsSuccess);