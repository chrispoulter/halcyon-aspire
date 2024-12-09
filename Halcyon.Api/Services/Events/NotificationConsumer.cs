using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Services.Events;

public class NotificationConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<NotificationConsumer> logger
) : IConsumer<Batch<INotification>>
{
    public async Task Consume(ConsumeContext<Batch<INotification>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            logger.LogInformation(
                "Broadcasting notification, Message: {Message}, Data: {Data}",
                message.Message,
                message.Data
            );

            await eventHubContext.Clients.All.ReceiveNotification(
                message,
                context.CancellationToken
            );

            //switch (message.Type)
            //{
            //    case nameof(User):
            //        var groups = new[]
            //        {
            //            NotificationHub.GetGroupForRole(Role.SystemAdministrator),
            //            NotificationHub.GetGroupForRole(Role.UserAdministrator),
            //            NotificationHub.GetGroupForUser(message.Id),
            //        };

            //        logger.LogInformation(
            //            "Broadcasting entity changed event, Groups: {Groups}, EntityType: {EntityType}, EntityState: {EntityState}, EntityId: {EntityId}",
            //            groups,
            //            message.Type,
            //            message.State,
            //            message.Id
            //        );

            //        await eventHubContext
            //            .Clients.Groups(groups)
            //            .ReceiveNotification(message, context.CancellationToken);

            //        break;
            //}
        }
    }
}
