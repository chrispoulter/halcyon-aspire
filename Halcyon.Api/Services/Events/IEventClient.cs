namespace Halcyon.Api.Services.Events;

public interface IEventClient
{
    Task ReceiveEvent(EntityChangedEvent message, CancellationToken cancellationToken);
}
