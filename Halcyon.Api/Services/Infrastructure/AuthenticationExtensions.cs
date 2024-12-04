﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Halcyon.Api.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Halcyon.Api.Services.Infrastructure;

public static class AuthenticationExtensions
{
    public static IHostApplicationBuilder AddAuthenticationFromConfig(
        this IHostApplicationBuilder builder
    )
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(JwtSettings.SectionName, jwtSettings);

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)
                    ),
                };
            });

        return builder;
    }
}
