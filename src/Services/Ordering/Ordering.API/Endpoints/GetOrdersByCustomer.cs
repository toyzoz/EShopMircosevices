using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("orders/{customerId:guid}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));
                var response = result.Adapt<GetOrdersByCustomerResponse>();

                return Results.Ok(response);
            })
            .WithName("GetOrdersByCustomer").WithDescription("Get orders by Customer")
            .WithSummary("Get orders by Customer").Produces<GetOrdersByCustomerResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}

public record GetOrdersByCustomerRequest;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);