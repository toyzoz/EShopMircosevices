using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries;

public record GetOrderByNameQuery(string OrderName) : IQuery<GetOrderByNameResult>;

public record GetOrderByNameResult(IEnumerable<OrderDto> Orders);

public class GetOrderByNameQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetOrderByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrderByNameQuery request, CancellationToken cancellationToken)
    {
        var orderName = request.OrderName;
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.OrderName.Value.Contains(orderName))
            .OrderBy(x => x.OrderName.Value)
            .ToListAsync(cancellationToken);

        return new GetOrderByNameResult(orders.ToOrderDtoList());
    }
}