using System.Net.Mail;
using MailKit.Client;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Halcyon.Api.Services.Email;

public class EmailService(
    MailKitClientFactory clientFactory,
    ITemplateEngine templateEngine,
    IOptions<EmailSettings> emailSettings,
    ILogger<EmailService> logger
) : IEmailService
{
    private readonly EmailSettings emailSettings = emailSettings.Value;

    public async Task SendEmailAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default
    )
    {
        logger.LogInformation(
            "Sending email to {To} with template {Template}",
            message.To,
            message.Template
        );

        var (body, subject) = await templateEngine.RenderTemplateAsync(
            message.Template,
            message.Data,
            cancellationToken
        );

        try
        {
            var client = await clientFactory.GetSmtpClientAsync(cancellationToken);

            using var email = new MailMessage(emailSettings.NoReplyAddress, message.To)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            await client.SendAsync(MimeMessage.CreateFromMailMessage(email), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while sending email to {To} with template {Template}",
                message.To,
                message.Template
            );
        }
    }
}
