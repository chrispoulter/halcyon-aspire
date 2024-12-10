namespace Halcyon.Api.Services.Events;

public abstract class EntityWithEvents
{
    private readonly List<object> _events = [];

    public List<object> Events => [.. _events];

    public void ClearEvents()
    {
        _events.Clear();
    }

    public void Raise(params object[] events)
    {
        _events.AddRange(events);
    }
}
