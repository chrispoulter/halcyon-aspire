using FluentValidation;
using Halcyon.Api.Data;
using Halcyon.Api.Services.Authentication;
using Halcyon.Api.Services.Database;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Events;
using Halcyon.Api.Services.Infrastructure;
using Mapster;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<HalcyonDbContext>(
    (provider, options) =>
    {
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("Database"))
            .UseSnakeCaseNamingConvention()
            .AddInterceptors(provider.GetServices<IInterceptor>());
    }
);

builder.EnrichNpgsqlDbContext<HalcyonDbContext>();

var seedConfig = builder.Configuration.GetSection(SeedSettings.SectionName);
builder.Services.Configure<SeedSettings>(seedConfig);
builder.Services.AddMigration<HalcyonDbContext, HalcyonDbSeeder>();

builder.AddMassTransit(connectionName: "RabbitMq", assembly);
builder.AddRedisDistributedCache(connectionName: "Redis");
builder.AddMailKitClient(connectionName: "Mail");

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

TypeAdapterConfig.GlobalSettings.Scan(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddProblemDetails();

builder.ConfigureJsonOptions();
builder.AddAuthentication();
builder.AddCors();
builder.AddSignalR();
builder.AddOpenApi();
builder.AddAuthenticationServices();
builder.AddEmailServices();
builder.AddEventServices();

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApiWithSwagger();
app.MapEndpoints(assembly);
app.MapHubs(assembly);
app.MapDefaultEndpoints();

app.Logger.LogInformation("Database Connection String {Database}", builder.Configuration.GetConnectionString("Database"));
app.Logger.LogInformation("RabbitMq Connection String {RabbitMq}", builder.Configuration.GetConnectionString("RabbitMq"));
app.Logger.LogInformation("Redis Connection String {Redis}", builder.Configuration.GetConnectionString("Redis"));
app.Logger.LogInformation("Mail Connection String {Mail}", builder.Configuration.GetConnectionString("Mail"));

app.Run();
