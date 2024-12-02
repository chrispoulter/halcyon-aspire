namespace Halcyon.Api.Services.Validation;

public static class ValidationFilterExtensions
{
    public static RouteHandlerBuilder AddValidationFilter<T>(this RouteHandlerBuilder builder)
        where T : class, new()
    {
        return builder.AddEndpointFilter<RouteHandlerBuilder, ValidationFilter<T>>();
    }
}
