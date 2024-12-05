namespace Halcyon.Api.Features.Account.SendResetPasswordEmail;

public class SendResetPasswordEmailEvent
{
    public string To { get; set; }

    public Guid? PasswordResetToken { get; set; }
}
