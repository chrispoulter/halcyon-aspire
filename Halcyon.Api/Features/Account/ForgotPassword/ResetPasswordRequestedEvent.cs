namespace Halcyon.Api.Features.Account.ForgotPassword;

public record ResetPasswordRequestedEvent(string EmailAddress, Guid? PasswordResetToken) { }
