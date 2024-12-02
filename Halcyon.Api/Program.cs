using Halcyon.Api.Data;
using Halcyon.Api.Extensions;
using Halcyon.Api.Features.Weather.GetWeatherForecast;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddHalcyonCors();

builder.AddNpgsqlDbContext<HalcyonDbContext>(connectionName: "postgresdb");
builder.AddRabbitMQClient(connectionName: "messaging");
builder.AddRedisDistributedCache(connectionName: "cache");

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

builder.Services.AddHalcyonOpenApi();

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors();
app.MapHalcyonOpenApi();

app.MapGetWeatherForecastEndpoint();

app.MapDefaultEndpoints();
app.Run();
