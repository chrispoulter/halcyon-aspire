using Halcyon.Api.Features.Users.CreateUser;
using Halcyon.Api.Services.Events;

namespace Halcyon.Api.Features.Users.DeleteUser;

public record UserDeletedEvent(Guid Id) : INotification
{
    public string Message => "User deleted";

    public Dictionary<string, object> Data =>
        new() { { "Id", Id }, { "Type", nameof(UserDeletedEvent) } };
}
