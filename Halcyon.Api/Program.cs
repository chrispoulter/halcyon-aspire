using FluentValidation;
using Halcyon.Api.Data;
using Halcyon.Api.Extensions;
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
    connectionName: "postgresdb",
    configureDbContextOptions: options => options.UseSnakeCaseNamingConvention()
);
builder.Services.AddMigration<HalcyonDbContext, HalcyonDbSeeder>();

builder.AddRabbitMQClient(connectionName: "messaging");
builder.AddRedisDistributedCache(connectionName: "cache");

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

TypeAdapterConfig.GlobalSettings.Scan(assembly);
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddEmailServices(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddHalcyonCors();
builder.Services.AddHalcyonOpenApi();
builder.Services.AddEndpoints(assembly);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHalcyonOpenApi();
app.MapEndpoints();
app.MapDefaultEndpoints();

app.Run();
