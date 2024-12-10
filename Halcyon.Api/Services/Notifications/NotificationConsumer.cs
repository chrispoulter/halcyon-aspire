using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Services.Notifications;

public class NotificationConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotificationConsumer> logger
) : IConsumer<Batch<NotificationEvent>>
{
    public async Task Consume(ConsumeContext<Batch<NotificationEvent>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            logger.LogInformation(
                "Broadcasting notification event, Groups: {Groups}, Users: {Users}, Notification: {Notification}",
                message.Roles,
                message.Users,
                message.Notification
            );

            var groups = message
                .Roles.Select(NotificationHub.GetGroupForRole)
                .Concat(message.Users.Select(id => NotificationHub.GetGroupForUser(id)));

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(message, context.CancellationToken);
        }
    }
}
