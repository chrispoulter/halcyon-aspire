using Halcyon.Api.Data.Users;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class NotifyUserChangedConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotifyUserChangedConsumer> logger
)
    : IConsumer<Batch<UserCreatedEvent>>,
        IConsumer<Batch<UserUpdatedEvent>>,
        IConsumer<Batch<UserDeletedEvent>>
{
    public Task Consume(ConsumeContext<Batch<UserCreatedEvent>> context) =>
        Consume(context, m => m.Id);

    public Task Consume(ConsumeContext<Batch<UserUpdatedEvent>> context) =>
        Consume(context, m => m.Id);

    public Task Consume(ConsumeContext<Batch<UserDeletedEvent>> context) =>
        Consume(context, m => m.Id);

    public async Task Consume<T>(ConsumeContext<Batch<T>> context, Func<T, Guid> getUserIdFn)
        where T : class
    {
        foreach (var message in context.Message.Select(m => m.Message))
        {
            var eventType = message.GetType().Name;

            var userId = getUserIdFn(message);

            var groups = new[]
            {
                NotificationHub.GetGroupForRole(Roles.SystemAdministrator),
                NotificationHub.GetGroupForRole(Roles.UserAdministrator),
                NotificationHub.GetGroupForUser(userId),
            };

            logger.LogInformation(
                "Sending notification for {Event} to {Groups}, UserId: {UserId}",
                eventType,
                groups,
                userId
            );

            var notification = new Notification(eventType, message);

            await eventHubContext
                .Clients.Groups(groups)
                .ReceiveNotification(notification, context.CancellationToken);
        }
    }
}
