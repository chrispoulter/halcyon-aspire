using Halcyon.Api.Services.Events;

namespace Halcyon.Api.Features.Notifications;

public interface INotificationClient
{
    Task ReceiveNotification(Notification notification, CancellationToken cancellationToken);
}
