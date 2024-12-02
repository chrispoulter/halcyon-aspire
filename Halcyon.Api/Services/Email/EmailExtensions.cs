﻿namespace Halcyon.Api.Services.Email;

public static class EmailExtensions
{
    public static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var emailConfig = configuration.GetSection(EmailSettings.SectionName);
        services.Configure<EmailSettings>(emailConfig);
        services.AddSingleton<IEmailSender, EmailSender>();
        services.AddSingleton<ITemplateEngine, TemplateEngine>();

        return services;
    }
}
