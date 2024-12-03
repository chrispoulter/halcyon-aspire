namespace Halcyon.Api.Services.Infrastructure;

public static class CorsExtensions
{
    public static IHostApplicationBuilder AddCorsFromConfig(this IHostApplicationBuilder builder)
    {
        var corsSettings = new CorsSettings();
        builder.Configuration.GetSection(CorsSettings.SectionName).Bind(corsSettings);

        builder.Services.AddCors(options =>
            options.AddDefaultPolicy(policy =>
                policy
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins(corsSettings.AllowedOrigins)
                    .WithMethods(corsSettings.AllowedMethods)
                    .WithHeaders(corsSettings.AllowedHeaders)
                    .AllowCredentials()
            )
        );

        return builder;
    }
}
