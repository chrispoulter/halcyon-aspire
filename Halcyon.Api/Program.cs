using FluentValidation;
using Halcyon.Api.Data;
using Halcyon.Api.Features;
using Halcyon.Api.Services.Database;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Infrastructure;
using Halcyon.Api.Services.Jwt;
using Mapster;
using Microsoft.EntityFrameworkCore;

var assembly = typeof(Program).Assembly;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<HalcyonDbContext>(
    connectionName: "Database",
    configureDbContextOptions: options => options.UseSnakeCaseNamingConvention()
);

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
builder.AddOpenApi();
builder.AddJwtServices();
builder.AddEmailServices();

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy(
        nameof(AuthPolicy.IsUserAdministrator),
        policy => policy.RequireRole(AuthPolicy.IsUserAdministrator)
    );

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapEndpoints(assembly);
app.MapDefaultEndpoints();

app.Run();
