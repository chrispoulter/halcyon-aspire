using System.Reflection;

namespace Halcyon.Api.Services.Infrastructure;

public static class EndpointExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app, Assembly assembly)
    {
        var endpointType = typeof(IEndpoint);

        var endpoints = assembly.DefinedTypes.Where(type =>
            type is { IsAbstract: false, IsInterface: false } && endpointType.IsAssignableFrom(type)
        );

        foreach (var endpoint in endpoints)
        {
            if (Activator.CreateInstance(endpoint) is IEndpoint instance)
            {
                instance.MapEndpoints(app);
            }
        }

        return app;
    }
}
