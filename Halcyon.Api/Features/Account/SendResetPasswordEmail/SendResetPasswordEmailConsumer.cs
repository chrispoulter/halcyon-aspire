using Halcyon.Api.Services.Email;
using MassTransit;

namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailConsumer(IEmailService emailService)
    : IConsumer<Batch<SendResetPasswordEmailEvent>>
{
    public async Task Consume(ConsumeContext<Batch<SendResetPasswordEmailEvent>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            await emailService.SendEmailAsync(
                new()
                {
                    Template = "ResetPasswordEmail.html",
                    To = message.To,
                    Data = new { message.PasswordResetToken },
                },
                context.CancellationToken
            );
        }
    }
}
