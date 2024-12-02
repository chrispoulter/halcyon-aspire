﻿namespace Halcyon.Api.Services.Auth;

public record CurrentUser(Guid Id)
{
    public static ValueTask<CurrentUser> BindAsync(HttpContext httpContext) =>
        Guid.TryParse(httpContext.User.Identity?.Name, out var id)
            ? ValueTask.FromResult(new CurrentUser(id))
            : ValueTask.FromResult<CurrentUser>(null);
}
