namespace Halcyon.Api.Services.Notifications;

public interface INotificationClient
{
    Task ReceiveNotification(object notification, CancellationToken cancellationToken);
}
