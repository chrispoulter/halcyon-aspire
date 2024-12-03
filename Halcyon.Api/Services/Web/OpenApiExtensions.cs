using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Halcyon.Api.Services.Web;

public static class OpenApiExtensions
{
    public static IHostApplicationBuilder AddOpenApiFromConfig(this IHostApplicationBuilder builder)
    {
        var openApiSettings = new OpenApiSettings();
        builder.Configuration.Bind(OpenApiSettings.SectionName, openApiSettings);

        builder.Services.AddOpenApi(
            openApiSettings.Version,
            options =>
            {
                options.AddDocumentTransformer(
                    (document, context, cancellationToken) =>
                    {
                        document.Info = new()
                        {
                            Version = openApiSettings.Version,
                            Title = openApiSettings.Title,
                            Description = openApiSettings.Description,
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

    public static WebApplication MapOpenApiWithSwagger(this WebApplication app)
    {
        var openApiSettings = app.Services.GetRequiredService<IOptions<OpenApiSettings>>().Value;

        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint(
                $"/openapi/{openApiSettings.Version}.json",
                openApiSettings.Version
            );
            options.DocumentTitle = openApiSettings.Title;
            options.RoutePrefix = string.Empty;
        });

        return app;
    }
}
