namespace Halcyon.Api.Services.Notifications;

public class NotificationEvent
{
    public object Notification { get; set; }

    public string[] Roles { get; } = [];

    public Guid[] Users { get; } = [];
}
