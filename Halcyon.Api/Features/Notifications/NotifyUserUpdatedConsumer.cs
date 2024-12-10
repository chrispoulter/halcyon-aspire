using Halcyon.Api.Data;
using Halcyon.Api.Events;
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
        foreach (var item in context.Message)
        {
            var message = item.Message;

            logger.LogInformation(
                "Sending notification for {Event}, Id: {Id}",
                nameof(UserUpdatedEvent),
                message.Id
            );

            var groups = new[]
            {
                NotificationHub.GetGroupForRole(Role.SystemAdministrator),
                NotificationHub.GetGroupForRole(Role.UserAdministrator),
                NotificationHub.GetGroupForUser(message.Id),
            };

            var notification = new Notification(nameof(User), "Updated", message.Id);

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
