using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public record GetOrdersByCustomerQuery(Guid CustomerId) : IQuery<GetOrdersByCustomerResult>;

public record GetOrdersByCustomerResult(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomerQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerId == CustomerId.Of(request.CustomerId))
            .ToListAsync(cancellationToken);

        return new GetOrdersByCustomerResult(orders.ToOrderDtoList());
    }
}