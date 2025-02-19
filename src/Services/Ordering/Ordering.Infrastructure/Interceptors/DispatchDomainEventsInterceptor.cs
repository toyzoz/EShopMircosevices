using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context is null) return;


        var aggregates = context.ChangeTracker.Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity)
            .ToList();

        var domainEvents = aggregates.SelectMany(a => a.DomainEvents).ToList();

        aggregates.ForEach(x => x.ClearDomainEvents());

        foreach (var domainEvent in domainEvents) await mediator.Publish(domainEvent);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}