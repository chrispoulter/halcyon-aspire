using Halcyon.Api.Data;
using Halcyon.Api.Features.Account.ForgotPassword;
using Halcyon.Api.Services.Email;
using MassTransit;

namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailConsumer(HalcyonDbContext dbContext, IEmailService emailService)
    : IConsumer<Batch<ResetPasswordRequestedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<ResetPasswordRequestedEvent>> context)
    {
        foreach (var message in context.Message.Select(m => m.Message))
        {
            var user = await dbContext.Users.FindAsync(message.Id);

            if (user is null || user.IsLockedOut || user.PasswordResetToken is null)
            {
                continue;
            }

            var email = new EmailMessage(
                "ResetPasswordEmail.html",
                user.EmailAddress,
                new { user.PasswordResetToken }
            );

            await emailService.SendEmailAsync(email, context.CancellationToken);
        }
    }
}
