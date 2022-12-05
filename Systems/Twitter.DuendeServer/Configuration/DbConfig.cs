using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;
using Twitter.Settings.Interfaces;

namespace Twitter.DuendeServer.Configuration;

public static class DbConfig
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbSettings settings)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(settings.GetConnectionString);
        });

        return services;
    }

    public static IApplicationBuilder UseAppDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}