using Microsoft.AspNetCore.Identity;
using Twitter.Context.Context;
using Twitter.Entities.Users;

namespace Twitter.DuendeServer.Configuration;

public static class DuendeConfiguration
{
    public static IServiceCollection AddDuende(this IServiceCollection services)
    {
        services.AddIdentity<TwitterUser, TwitterRole>(opt =>
            {
                opt.Password.RequiredLength = 0;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MainDbContext>()
            .AddUserManager<UserManager<TwitterUser>>()
            .AddSignInManager<SignInManager<TwitterUser>>()
            .AddDefaultTokenProviders();


        services.AddIdentityServer(options => { options.EmitStaticAudienceClaim = true; })
            .AddAspNetIdentity<TwitterUser>()
            .AddInMemoryApiScopes(DuendeConfig.Scopes)
            .AddInMemoryIdentityResources(DuendeConfig.Resources)
            .AddInMemoryClients(DuendeConfig.Clients)
            .AddDeveloperSigningCredential();
        //.AddTestUsers(DuendeConfig.Users);

        return services;
    }

    public static WebApplication UseDuende(this WebApplication app)
    {
        app.UseIdentityServer();
        return app;
    }
}