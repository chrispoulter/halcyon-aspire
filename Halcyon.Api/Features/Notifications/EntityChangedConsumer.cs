using Halcyon.Api.Data;
using Halcyon.Api.Services.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Features.Notifications;

public class EntityChangedConsumer(
    IHubContext<NotificationHub, INotificationClient> eventHubContext,
    ILogger<EntityChangedConsumer> logger
) : IConsumer<Batch<EntityChangedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<EntityChangedEvent>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            switch (message.Type)
            {
                case nameof(User):
                    var groups = new[]
                    {
                        NotificationHub.GetGroupForRole(Role.SystemAdministrator),
                        NotificationHub.GetGroupForRole(Role.UserAdministrator),
                        NotificationHub.GetGroupForUser(message.Id),
                    };

                    logger.LogInformation(
                        "Broadcasting entity changed event, Groups: {Groups}, EntityType: {EntityType}, EntityState: {EntityState}, EntityId: {EntityId}",
                        groups,
                        message.Type,
                        message.State,
                        message.Id
                    );

                    await eventHubContext
                        .Clients.Groups(groups)
                        .ReceiveEvent(message, context.CancellationToken);

                    break;
            }
        }
    }
}
