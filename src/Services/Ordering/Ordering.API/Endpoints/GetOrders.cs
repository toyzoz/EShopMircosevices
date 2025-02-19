using BuildingBlocks.Pagination;
using Carter;
using Mapster;
using MediatR;
using Ordering.Application.Orders.Queries.GetOrders;
using Ordering.Domain.Models;

namespace Ordering.API.Endpoints;

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders", async (PaginatedRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetOrdersQuery>();
                var result = await sender.Send(query);
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

public record GetOrdersResponse(PaginatedResult<Order> Orders);