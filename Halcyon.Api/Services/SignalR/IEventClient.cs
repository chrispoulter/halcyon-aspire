namespace Halcyon.Api.Services.SignalR;

public interface IEventClient
{
    Task ReceiveEvent(EntityChangedEvent message, CancellationToken cancellationToken);
}
