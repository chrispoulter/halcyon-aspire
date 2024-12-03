namespace Halcyon.Api.Services.Web;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsFromConfig(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var corsSettings = new CorsSettings();
        configuration.GetSection(CorsSettings.SectionName).Bind(corsSettings);

        services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(corsSettings.AllowedOrigins)
                    .WithMethods(corsSettings.AllowedMethods)
                    .WithHeaders(corsSettings.AllowedHeaders)
                    .AllowCredentials()
            )
        );

        return services;
    }
}
