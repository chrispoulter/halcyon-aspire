using Halcyon.Api.Services.Web;

namespace Halcyon.Api.Features.Weather.GetWeatherForecast;

public class GetWeatherForecastEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        string[] summaries =
        [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching",
        ];

        app.MapGet(
                "/weatherforecast",
                () =>
                {
                    var forecast = Enumerable
                        .Range(1, 5)
                        .Select(index => new WeatherForecast(
                            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                            Random.Shared.Next(-20, 55),
                            summaries[Random.Shared.Next(summaries.Length)]
                        ))
                        .ToArray();
                    return forecast;
                }
            )
            .WithName("GetWeatherForecast");
    }
}

record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
