using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Halcyon.Api.Extensions;

public static class OpenApiExtensions
{
    public static IServiceCollection AddHalcyonOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi(
            "v1",
            options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Info = new()
                        {
                            Version = "v1",
                            Title = "Halcyon API",
                            Description =
                                "A .NET Core REST API project template. Built with a sense of peace and tranquillity.",
                        };

                        document.Servers.Clear();

                        return Task.CompletedTask;
                    }
                );

                var scheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Name = JwtBearerDefaults.AuthenticationScheme,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Reference = new()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme,
                    },
                };

                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Components ??= new();
                        document.Components.SecuritySchemes.Add(
                            JwtBearerDefaults.AuthenticationScheme,
                            scheme
                        );
                        return Task.CompletedTask;
                    }
                );

                options.AddOperationTransformer(
                    (operation, context, cancellationToken) =>
                    {
                        if (
                            context
                                .Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>()
                                .Any()
                        )
                        {
                            operation.Security = [new() { [scheme] = [] }];
                        }
                        return Task.CompletedTask;
                    }
                );
            }
        );

        return services;
    }

    public static WebApplication MapHalcyonOpenApi(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/openapi/v1.json", "v1");
            options.DocumentTitle = "Halcyon API";
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
