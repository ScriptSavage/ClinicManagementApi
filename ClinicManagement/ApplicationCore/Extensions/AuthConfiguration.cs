using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationCore.Extensions;

public static class AuthConfiguration
{
    private const string RoleClaimType = "role";

    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddRoles<ApplicationRole>()
            .AddSignInManager()
            .AddEntityFrameworkStores<DatabaseContext>();

        var jwtSettings = configuration.GetRequiredSection("JwtSettings");

        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Issuer is missing");

        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Audience is missing");

        var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("Secret is missing");

        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                        ValidateLifetime = true,
                        RequireExpirationTime = true,

                        RoleClaimType = RoleClaimType,
                        NameClaimType = JwtRegisteredClaimNames.UniqueName,

                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
            });

        services.AddAuthorization();
    }
}