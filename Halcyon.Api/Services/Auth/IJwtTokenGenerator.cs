using Halcyon.Api.Data;

namespace Halcyon.Api.Services.Auth;

public interface IJwtTokenGenerator
{
    public string GenerateJwtToken(User user);
}
