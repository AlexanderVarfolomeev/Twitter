using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;
using Twitter.Context.Factories;
using Twitter.Settings.Interfaces;
using Twitter.Settings.Source;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static partial class DbConfiguration
{
    public static IServiceCollection AddTwitterDbContext(this IServiceCollection services, ITwitterApiSettings dbSettings)
    {
        /*
        var dbOptDelegate = DbContextOptionsFactories.Configure(dbSettings.Db.GetConnectionString);
        services.AddDbContextFactory<MainDbContext>(dbOptDelegate, ServiceLifetime.Singleton);
        */
        
        var a = dbSettings.Db.GetConnectionString;
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseSqlServer(dbSettings.Db.GetConnectionString);
        });

        return services;
    }
    
}