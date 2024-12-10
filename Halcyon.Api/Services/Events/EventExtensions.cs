using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Halcyon.Api.Services.Events;

public static class EventExtensions
{
    public static IHostApplicationBuilder AddEventServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IInterceptor, EntityEventInteceptor>();

        return builder;
    }
}
