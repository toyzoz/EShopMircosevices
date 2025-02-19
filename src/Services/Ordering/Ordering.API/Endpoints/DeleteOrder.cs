using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Orders.Commands;

namespace Ordering.API.Endpoints;

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/order/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(id));
                var res = result.Adapt<DeleteOrderResponse>();

                return Results.Ok(res);
            })
            .WithName("delete order").WithDescription("delete order").WithSummary("delete order")
            .Produces<DeleteOrderResponse>().ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

public record DeleteOrderRequest(Guid Id);

public record DeleteOrderResponse(bool IsSuccess);