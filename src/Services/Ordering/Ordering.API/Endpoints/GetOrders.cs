using BuildingBlocks.Pagination;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async ([AsParameters] PaginatedRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));
                var response = result.Adapt<GetOrdersResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrders")
            .WithDescription("Get all orders")
            .WithSummary("Get all orders").Produces<GetOrdersRequest>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record GetOrdersRequest;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);