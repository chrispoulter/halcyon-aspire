using Halcyon.Api.Services.Events;

namespace Halcyon.Api.Features.Notifications;

public interface INotificationClient
{
    Task ReceiveEvent(EntityChangedEvent message, CancellationToken cancellationToken);
}
