namespace Halcyon.Api.Services.Web;

public class CorsSettings
{
    public static string SectionName { get; } = "Cors";

    public string[] AllowedOrigins { get; set; }

    public string[] AllowedMethods { get; set; }

    public string[] AllowedHeaders { get; set; }
}
