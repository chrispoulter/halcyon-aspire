using NpgsqlTypes;

namespace Halcyon.Api.Data;

public class User
{
    public Guid Id { get; set; }

    public required string EmailAddress { get; set; }

    public string? Password { get; set; }

    public Guid? PasswordResetToken { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public bool IsLockedOut { get; set; }

    public required List<string> Roles { get; set; }

    public uint Version { get; set; }

    public required NpgsqlTsVector SearchVector { get; set; }
}
