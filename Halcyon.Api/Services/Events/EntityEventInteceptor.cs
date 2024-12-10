using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Halcyon.Api.Services.Events;

public class EntityEventInteceptor(IPublishEndpoint publishEndpoint) : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishEntityEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default
    )
    {
        await PublishEntityEvents(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishEntityEvents(
        DbContext context,
        CancellationToken cancellationToken = default
    )
    {
        var events = context
            .ChangeTracker.Entries<EntityWithEvents>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                var events = entity.Events;
                entity.ClearEvents();
                return events;
            })
            .ToList();

        foreach (var @event in events)
        {
            await publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}
