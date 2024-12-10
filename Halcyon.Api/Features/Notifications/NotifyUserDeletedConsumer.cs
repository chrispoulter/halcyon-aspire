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
        foreach (var item in context.Message)
        {
            var message = item.Message;

            logger.LogInformation(
                "Sending notification for {Event}, Id: {Id}",
                nameof(UserDeletedEvent),
                message.Id
            );

            var groups = new[]
            {
                NotificationHub.GetGroupForRole(Role.SystemAdministrator),
                NotificationHub.GetGroupForRole(Role.UserAdministrator),
                NotificationHub.GetGroupForUser(message.Id),
            };

            var notification = new Notification(nameof(User), "Deleted", message.Id);

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
