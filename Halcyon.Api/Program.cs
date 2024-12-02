using Halcyon.Api.Data;
using Halcyon.Api.Extensions;

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
builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors();

app.MapHalcyonOpenApi();
app.MapEndpoints();
app.MapDefaultEndpoints();

app.Run();
