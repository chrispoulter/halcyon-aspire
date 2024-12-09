using Halcyon.Api.Services.Events;

namespace Halcyon.Api.Features.Users.CreateUser;

public record UserCreatedEvent(Guid Id) : INotification
{
    public string Message => "User created";

    public Dictionary<string, object> Data =>
        new() { { "Id", Id }, { "Type", nameof(UserCreatedEvent) } };
}
