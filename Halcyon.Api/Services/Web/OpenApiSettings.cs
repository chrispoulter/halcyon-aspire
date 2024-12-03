namespace Halcyon.Api.Services.Web;

public class OpenApiSettings
{
    public static string SectionName { get; } = "OpenApi";

    public string Version { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}
