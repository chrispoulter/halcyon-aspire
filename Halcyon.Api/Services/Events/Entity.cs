namespace Halcyon.Api.Services.Events;

public abstract class Entity
{
    private readonly List<object> events = [];

    public List<object> Events => [.. events];

    public void ClearEvents()
    {
        events.Clear();
    }

    public void Raise(params object[] raiseEvents)
    {
        events.AddRange(raiseEvents);
    }
}
