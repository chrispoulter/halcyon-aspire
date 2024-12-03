namespace Halcyon.Api.Services.Email;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var emailConfig = configuration.GetSection(EmailSettings.SectionName);
        services.Configure<EmailSettings>(emailConfig);
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<ITemplateEngine, TemplateEngine>();

        return services;
    }
}
