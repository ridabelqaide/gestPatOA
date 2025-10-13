using PATOA.INFRA.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace PATOA.WebAPI.DI.Installers;

public static class AuthenticationInstaller
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add JWT settings
        var jwtSettings = new JwtSettings();
        configuration.GetSection("Jwt").Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        // Configure JWT Authentication
        services.AddAuthentication()
            .AddJwtBearer("AdminScheme", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = ClaimTypes.Role,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.AdminToken.SecretKey ??
                            throw new InvalidOperationException("Admin JWT Secret Key not found in configuration"))),
                    ClockSkew = TimeSpan.Zero // Pour une expiration exacte
                };
            });
          

         
       

        return services;
    }
}