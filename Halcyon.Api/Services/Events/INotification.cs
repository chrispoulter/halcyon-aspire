namespace Halcyon.Api.Services.Events;

public interface INotification
{
    public string Message { get; }

    public Dictionary<string, object> Data { get; }
}
