using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Halcyon.Api.Services.Events;

public class EventInterceptor(IPublishEndpoint publishEndpoint) : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default
    )
    {
        await PublishEvents(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishEvents(
        DbContext context,
        CancellationToken cancellationToken = default
    )
    {
        var events = context
            .ChangeTracker.Entries<Entity>()
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
