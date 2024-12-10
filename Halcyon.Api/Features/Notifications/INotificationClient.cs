namespace Halcyon.Api.Features.Notifications;

public interface INotificationClient
{
    Task ReceiveNotification(object notification, CancellationToken cancellationToken);
}
