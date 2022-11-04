using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;
using Twitter.Settings.Interfaces;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static class DbConfiguration
{
    public static IServiceCollection AddTwitterDbContext(this IServiceCollection services,
        ITwitterApiSettings dbSettings)
    {
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(dbSettings.Db.GetConnectionString);
        });

        return services;
    }
}