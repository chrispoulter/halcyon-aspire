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
        var ids = context.Message.Select(m => m.Message.Id);

        var users = await dbContext
            .Users.Where(u => ids.Contains(u.Id) && !u.IsLockedOut && u.PasswordResetToken != null)
            .ToListAsync(context.CancellationToken);

        foreach (var user in users)
        {
            var email = new EmailMessage(
                "ResetPasswordEmail.html",
                user.EmailAddress,
                new { user.PasswordResetToken }
            );

            await emailService.SendEmailAsync(email, context.CancellationToken);
        }
    }
}
