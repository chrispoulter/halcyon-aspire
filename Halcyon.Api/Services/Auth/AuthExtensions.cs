namespace Halcyon.Api.Services.Auth;

public static class AuthExtensions
{
    public static IHostApplicationBuilder AddAuthServices(this IHostApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection(JwtSettings.SectionName);
        builder.Services.Configure<JwtSettings>(jwtConfig);
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return builder;
    }
}
