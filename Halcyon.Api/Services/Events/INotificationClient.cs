namespace Halcyon.Api.Services.Events;

public interface INotificationClient
{
    Task ReceiveNotification(INotification notification, CancellationToken cancellationToken);
}
