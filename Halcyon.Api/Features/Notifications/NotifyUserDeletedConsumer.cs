using Halcyon.Api.Data;
using Halcyon.Api.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class NotifyUserDeletedConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotifyUserDeletedConsumer> logger
) : IConsumer<Batch<UserDeletedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<UserDeletedEvent>> context)
    {
        foreach (var message in context.Message.Select(m => m.Message))
        {
            var groups = new[]
            {
                NotificationHub.GetGroupForRole(Role.SystemAdministrator),
                NotificationHub.GetGroupForRole(Role.UserAdministrator),
                NotificationHub.GetGroupForUser(message.Id),
            };

            logger.LogInformation(
                "Sending notification for {Event} to {Groups}",
                nameof(UserDeletedEvent),
                groups
            );

            var notification = new Notification(nameof(UserDeletedEvent), message);

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
