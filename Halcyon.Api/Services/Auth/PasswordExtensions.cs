namespace Halcyon.Api.Services.Auth;

public static class PasswordExtensions
{
    public static IHostApplicationBuilder AddPasswordServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

        return builder;
    }
}
