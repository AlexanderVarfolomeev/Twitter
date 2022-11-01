using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Twitter.Context.Context;
using Twitter.Entities.Users;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static partial class AuthConfiguration
{
    public static IServiceCollection AddTwitterAuth(this IServiceCollection services)
    {
        services.AddIdentity<TwitterUser, TwitterRole>(options =>
            {
                //TODO исправить требования
                options.Password.RequiredLength = 0; 
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
               // options.User.RequireUniqueEmail
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<TwitterUser>>()
            .AddSignInManager<SignInManager<TwitterUser>>()
            .AddRoleManager<RoleManager<TwitterRole>>()
            .AddRoles<TwitterRole>()
            .AddDefaultTokenProviders();
     
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;

        })
            .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false;
            options.Authority = "https://localhost:5001"; //TODO settings
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Audience = "api";
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("twitter_api", policy => policy.RequireClaim("scope", "twitter_api"));
        });

        return services;
    }
}