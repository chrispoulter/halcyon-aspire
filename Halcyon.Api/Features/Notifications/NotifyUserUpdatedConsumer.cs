using Halcyon.Api.Data;
using Halcyon.Api.Data.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class NotifyUserUpdatedConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotifyUserUpdatedConsumer> logger
) : IConsumer<Batch<UserUpdatedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<UserUpdatedEvent>> context)
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
                nameof(UserUpdatedEvent),
                groups
            );

            var notification = new Notification(nameof(UserUpdatedEvent), message);

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
