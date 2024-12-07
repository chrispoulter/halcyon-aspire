using Halcyon.Api.Data;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace Halcyon.Api.Services.SignalR;

public class EntityChangedConsumer(IHubContext<EventHub, IEventClient> eventHubContext)
    : IConsumer<Batch<EntityChangedEvent>>
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

                    await eventHubContext
                        .Clients.Groups(groups)
                        .ReceiveEvent(message, context.CancellationToken);

                    break;
            }
        }
    }
}
