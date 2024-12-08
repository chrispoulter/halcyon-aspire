using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Halcyon.Api.Services.Infrastructure;

public static class EndpointExtensions
{
    public static IHostApplicationBuilder AddEndpoints(
        this IHostApplicationBuilder builder,
        Assembly assembly
    )
    {
        var serviceDescriptors = assembly
            .DefinedTypes.Where(type =>
                type is { IsAbstract: false, IsInterface: false }
                && type.IsAssignableTo(typeof(IEndpoint))
            )
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        builder.Services.TryAddEnumerable(serviceDescriptors);

        return builder;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }

        return app;
    }
}
