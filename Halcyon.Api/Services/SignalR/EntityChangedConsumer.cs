using Halcyon.Api.Data;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Services.SignalR;

public class EntityChangedConsumer(
    IHubContext<EventHub, IEventClient> eventHubContext,
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
                        EventHub.GetGroupForRole(Role.SystemAdministrator),
                        EventHub.GetGroupForRole(Role.UserAdministrator),
                        EventHub.GetGroupForUser(message.Id),
                    };

                    //using (
                    //    logger.BeginScope(
                    //        new List<KeyValuePair<string, object>>
                    //        {
                    //            new("Id", message.Id),
                    //            new("Type", message.Type),
                    //            new("State", message.State),
                    //            new("Timestamp", message.Timestamp),
                    //        }
                    //    )
                    //)
                    {
                        logger.LogInformation(
                            "Sending entity changed event {Event} to groups {Groups}",
                            message,
                            groups
                        );
                    }

                    await eventHubContext
                        .Clients.Groups(groups)
                        .ReceiveEvent(message, context.CancellationToken);

                    break;
            }
        }
    }
}
