﻿using Halcyon.Api.Features.Users.CreateUser;
using Halcyon.Api.Features.Users.UpdateUser;
using Halcyon.Api.Services.Authentication;
using Halcyon.Api.Services.Database;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Halcyon.Api.Data;

public class HalcyonDbSeeder(
    HalcyonDbContext dbContext,
    IPasswordHasher passwordHasher,
    IOptions<SeedSettings> seedSettings
) : IDbSeeder<HalcyonDbContext>
{
    private readonly SeedSettings _seedSettings = seedSettings.Value;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var emailAddresses = _seedSettings.Users.Select(u => u.EmailAddress);

        var users = await dbContext
            .Users.Where(u => emailAddresses.Contains(u.EmailAddress))
            .ToListAsync(cancellationToken);

        foreach (var seedUser in _seedSettings.Users)
        {
            var user = users.FirstOrDefault(u => u.EmailAddress == seedUser.EmailAddress);

            if (user is null)
            {
                user = new();
                user.Raise(new UserUpdatedEvent(user.Id));

                dbContext.Users.Add(user);
            }
            else
            {
                user.Raise(new UserCreatedEvent(user.Id));
            }

            seedUser.Adapt(user);
            user.Password = passwordHasher.HashPassword(seedUser.Password);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
