using Twitter.Context.Context;
using Twitter.Context.Factories;
using Twitter.Settings.Interfaces;

namespace Twitter.DuendeServer.Configuration;

public static class DbConfig
{
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, IDbSettings settings)
    {
        var dbOptionsDelegate = DbContextOptionsFactories.Configure(settings.GetConnectionString);

        services.AddDbContextFactory<MainDbContext>(dbOptionsDelegate, ServiceLifetime.Singleton);

        return services;
    }

    public static IApplicationBuilder UseAppDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}