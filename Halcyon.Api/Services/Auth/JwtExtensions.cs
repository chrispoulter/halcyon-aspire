namespace Halcyon.Api.Services.Auth;

public static class JwtExtensions
{
    public static IHostApplicationBuilder AddJwtServices(this IHostApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection(JwtSettings.SectionName);
        builder.Services.Configure<JwtSettings>(jwtConfig);
        builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return builder;
    }
}
