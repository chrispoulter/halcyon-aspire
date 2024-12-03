using FluentValidation;
using Halcyon.Api.Data;
using Halcyon.Api.Features;
using Halcyon.Api.Services.Auth;
using Halcyon.Api.Services.Database;
using Halcyon.Api.Services.Email;
using Halcyon.Api.Services.Web;
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

//builder.AddRabbitMQClient(connectionName: "RabbitMq");
builder.AddMassTransitWithRabbitMq(connectionName: "RabbitMq", assembly);
builder.AddRedisDistributedCache(connectionName: "Redis");

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

TypeAdapterConfig.GlobalSettings.Scan(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddProblemDetails();

builder.ConfigureJsonDefaults();
builder.AddAuthenticationFromConfig();
builder.AddCorsFromConfig();
builder.AddOpenApiFromConfig();
builder.AddAuthServices();
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

app.MapOpenApiWithSwagger();
app.MapEndpoints(assembly);
app.MapDefaultEndpoints();

app.Run();
