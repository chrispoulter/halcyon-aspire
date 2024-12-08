using Halcyon.Api.Data;

namespace Halcyon.Api.Services.Authentication;

public interface IJwtTokenGenerator
{
    public string GenerateJwtToken(User user);
}
