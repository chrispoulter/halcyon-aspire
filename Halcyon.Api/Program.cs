using Halcyon.Api.Data;
using Halcyon.Api.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddHalcyonCors();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.AddNpgsqlDbContext<HalcyonDbContext>(
    connectionName: "postgresdb",
    configureDbContextOptions: options => options.UseSnakeCaseNamingConvention()
);
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
app.UseAuthentication();
app.UseAuthorization();

app.MapHalcyonOpenApi();
app.MapEndpoints();
app.MapDefaultEndpoints();

app.Run();
