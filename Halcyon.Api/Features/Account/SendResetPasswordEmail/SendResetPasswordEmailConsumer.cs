using Halcyon.Api.Features.Account.ForgotPassword;
using Halcyon.Api.Services.Email;
using MassTransit;

namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailConsumer(IEmailService emailService)
    : IConsumer<Batch<ResetPasswordRequestedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<ResetPasswordRequestedEvent>> context)
    {
        foreach (var message in context.Message.Select(m => m.Message))
        {
            var email = new EmailMessage(
                "ResetPasswordEmail.html",
                message.EmailAddress,
                new { message.PasswordResetToken }
            );

            await emailService.SendEmailAsync(email, context.CancellationToken);
        }
    }
}
