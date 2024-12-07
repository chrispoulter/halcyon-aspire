using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Services.SignalR;

public class EntityChangedEvent
{
    public string Type { get; set; }

    public EntityState State { get; set; }

    public object Id { get; set; }

    public DateTime Timestamp { get; set; }
}
