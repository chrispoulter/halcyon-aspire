using Halcyon.Api.Services.Events;

namespace Halcyon.Api.Features.Users.UpdateUser;

public record UserUpdatedEvent(Guid Id) : INotification
{
    public string Message => "User updated";

    public Dictionary<string, object> Data =>
        new() { { "Id", Id }, { "Type", nameof(UserUpdatedEvent) } };
}
