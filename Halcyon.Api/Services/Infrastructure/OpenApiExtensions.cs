using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

namespace Halcyon.Api.Services.Infrastructure;

public static class OpenApiExtensions
{
    public const string Version = "v1";

    public const string Title = "Halcyon API";

    public const string Description =
        "A .NET Core REST API project template. Built with a sense of peace and tranquillity.";

    public static IHostApplicationBuilder AddOpenApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(
            Version,
            options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Info = new()
                        {
                            Version = Version,
                            Title = Title,
                            Description = Description,
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

        return builder;
    }

    public static WebApplication MapOpenApi(this WebApplication app)
    {
        OpenApiEndpointRouteBuilderExtensions.MapOpenApi(app);

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/openapi/{Version}.json", Version);
            options.DocumentTitle = Title;
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
