﻿using Microsoft.EntityFrameworkCore;

namespace Halcyon.Api.Services.Database;

public static class MigrationExtensions
{
    public static IServiceCollection AddMigration<TDbContext, TDbSeeder>(
        this IServiceCollection services
    )
        where TDbContext : DbContext
        where TDbSeeder : class, IDbSeeder<TDbContext>
    {
        services.AddHostedService<MigrationHostedService<TDbContext>>();
        services.AddScoped<IDbSeeder<TDbContext>, TDbSeeder>();
        return services;
    }
}
