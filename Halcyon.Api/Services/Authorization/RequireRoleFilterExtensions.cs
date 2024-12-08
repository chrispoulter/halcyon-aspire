namespace Halcyon.Api.Services.Authorization;

public static class RequireRoleFilterExtensions
{
    public static RouteHandlerBuilder RequireRole(
        this RouteHandlerBuilder builder,
        params string[] roles
    )
    {
        return builder.AddEndpointFilter(new RequireRoleFilter(roles));
    }
}
