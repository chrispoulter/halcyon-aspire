namespace Halcyon.Api.Services.Auth;

public static class AuthExtensions
{
    public static IServiceCollection AddAuthServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtConfig = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(jwtConfig);
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
