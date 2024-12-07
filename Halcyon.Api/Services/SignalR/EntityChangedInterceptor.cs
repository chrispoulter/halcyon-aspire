using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Halcyon.Api.Services.SignalR;

public class EntityChangedInterceptor(IPublishEndpoint publishEndpoint) : SaveChangesInterceptor
{
    private readonly List<EntityChangedEvent> _pendingEvents = [];

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        CaptureEntityChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        CaptureEntityChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishPendingEventsAsync().GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default
    )
    {
        await PublishPendingEventsAsync();
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void CaptureEntityChanges(DbContext context)
    {
        if (context == null)
        {
            return;
        }

        var entries = context
            .ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in entries)
        {
            _pendingEvents.Add(
                new EntityChangedEvent
                {
                    Type = entry.Entity.GetType().Name,
                    State = entry.State,
                    Id = GetPrimaryKeyValue(entry),
                    Timestamp = DateTime.UtcNow,
                }
            );
        }
    }

    private async Task PublishPendingEventsAsync()
    {
        await publishEndpoint.PublishBatch(_pendingEvents);
        _pendingEvents.Clear();
    }

    private static object GetPrimaryKeyValue(EntityEntry entry)
    {
        var keyProperties = entry.Metadata.FindPrimaryKey().Properties;

        if (keyProperties is null || keyProperties.Count == 0)
        {
            return null;
        }

        if (keyProperties.Count == 1)
        {
            return entry.Property(keyProperties[0].Name).CurrentValue;
        }

        return keyProperties.Select(p => entry.Property(p.Name).CurrentValue).ToArray();
    }
}
