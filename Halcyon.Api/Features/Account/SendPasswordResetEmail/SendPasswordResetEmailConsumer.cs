using Halcyon.Api.Data;
using Halcyon.Api.Features.Account.ForgotPassword;
using Halcyon.Api.Services.Email;
using MassTransit;

namespace Halcyon.Api.Features.Account.SendPasswordResetEmail;

public class SendPasswordResetEmailConsumer(HalcyonDbContext dbContext, IEmailService emailService)
    : IConsumer<Batch<PasswordResetRequestedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<PasswordResetRequestedEvent>> context)
    {
        foreach (var item in context.Message)
        {
            var message = item.Message;

            var user = await dbContext.Users.FindAsync(message.Id, context.CancellationToken);

            if (user is null || user.IsLockedOut || user.PasswordResetToken is null)
            {
                continue;
            }

            await emailService.SendEmailAsync(
                new()
                {
                    Template = "PasswordResetEmail.html",
                    To = user.EmailAddress,
                    Data = new { user.PasswordResetToken },
                },
                context.CancellationToken
            );
        }
    }
}
