using Halcyon.Api.Data;
using Halcyon.Api.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class UserEventConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<UserEventConsumer> logger
) : IConsumer<Batch<UserCreatedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<UserCreatedEvent>> context)
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

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(message, context.CancellationToken);
        }
    }
}
