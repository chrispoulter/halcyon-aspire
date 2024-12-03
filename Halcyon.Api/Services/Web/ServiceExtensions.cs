using System.Text.Json.Serialization;

namespace Halcyon.Api.Services.Web;

public static class ServiceExtensions
{
    public static IHostApplicationBuilder ConfigureApiDefaults(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return builder;
    }
}
