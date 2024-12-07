using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Halcyon.Api.Services.SignalR;

public static class SignalRExtensions
{
    public static IHostApplicationBuilder AddSignalR(this IHostApplicationBuilder builder)
    {
        builder
            .Services.AddSignalR()
            .AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
                options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        builder.Services.AddScoped<ISaveChangesInterceptor, EntityChangedInterceptor>();

        return builder;
    }

    public static WebApplication MapHubs(this WebApplication app)
    {
        app.MapHub<EventHub>(EventHub.Pattern);

        return app;
    }
}
