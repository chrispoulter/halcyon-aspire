namespace Halcyon.Api.Extensions;

public static class CorsExtensions
{
    public static IServiceCollection AddHalcyonCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        return services;
    }
}
