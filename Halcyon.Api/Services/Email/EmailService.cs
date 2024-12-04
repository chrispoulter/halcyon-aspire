using System.Net.Mail;
using MailKit.Client;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

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

            await client.SendAsync(MimeMessage.CreateFromMailMessage(email));
        }
        catch (Exception error)
        {
            logger.LogError(
                error,
                "An error occurred while sending email to {To} with template {Template}",
                message.To,
                message.Template
            );
        }
    }
}
