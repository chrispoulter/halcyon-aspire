using Halcyon.Api.Data;
using Halcyon.Api.Features.Account.ForgotPassword;
using Halcyon.Api.Services.Email;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailConsumer(HalcyonDbContext dbContext, IEmailService emailService)
    : IConsumer<Batch<ResetPasswordRequestedEvent>>
{
    public async Task Consume(ConsumeContext<Batch<ResetPasswordRequestedEvent>> context)
    {
        var userIds = context.Message.Select(m => m.Message.Id);

        var users = await dbContext
            .Users.Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, context.CancellationToken);

        foreach (var message in context.Message.Select(m => m.Message))
        {
            var user = users.GetValueOrDefault(message.Id);

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
