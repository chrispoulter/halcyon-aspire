﻿using Halcyon.Api.Data;
using Halcyon.Api.Events;
using Halcyon.Api.Services.Infrastructure;
using Halcyon.Api.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Features.Account.ForgotPassword;

public class ForgotPasswordEndpoint : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPut("/account/forgot-password", HandleAsync)
            .AddValidationFilter<ForgotPasswordRequest>()
            .WithTags(EndpointTag.Account)
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> HandleAsync(
        ForgotPasswordRequest request,
        HalcyonDbContext dbContext,
        CancellationToken cancellationToken = default
    )
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.EmailAddress == request.EmailAddress,
            cancellationToken
        );

        if (user is not null && !user.IsLockedOut)
        {
            user.PasswordResetToken = Guid.NewGuid();
            user.Raise(
                new UserUpdatedEvent(user.Id),
                new ResetPasswordRequestedEvent(user.EmailAddress, user.PasswordResetToken)
            );

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Results.Ok();
    }
}
