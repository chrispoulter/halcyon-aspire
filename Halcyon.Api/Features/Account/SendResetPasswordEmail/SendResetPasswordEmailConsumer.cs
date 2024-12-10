using Halcyon.Api.Features.Account.ForgotPassword;
using Halcyon.Api.Services.Email;
using MassTransit;

namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailConsumer(IEmailService emailService)
    : IConsumer<Batch<ResetPasswordRequestedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<ResetPasswordRequestedEvent>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            await emailService.SendEmailAsync(
                new()
                {
                    Template = "ResetPasswordEmail.html",
                    To = message.EmailAddress,
                    Data = new { message.PasswordResetToken },
                },
                context.CancellationToken
            );
        }
    }
}
