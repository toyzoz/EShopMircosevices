using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries;

namespace Ordering.API.Endpoints;

public class GetOrderByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByNameQuery(orderName));
                var res = result.Adapt<GetOrderByNameResponse>();

                return Results.Ok(res);
            })
            .WithName("GetOrderByName").WithDescription("Get Order By Name").WithSummary("Get Order By Name");
    }
}

public record GetOrderByNameRequest;

public record GetOrderByNameResponse(IEnumerable<OrderDto> Orders);