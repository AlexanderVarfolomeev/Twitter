using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;
using Twitter.Settings.Interfaces;
using Twitter.Settings.Source;

namespace Twitter.Api.Configuration;

public static class DbConfiguration
{
    public static IServiceCollection AddTwitterDbContext(this IServiceCollection services, ISettingSource dbSettings)
    {
        var a = dbSettings.GetConnectionString("MainConnectionString");
        services.AddDbContext<MainDbContext>(options =>
        {
            options.UseSqlServer(dbSettings.GetConnectionString("MainConnectionString"));
        });

        return services;
    }

    public static IApplicationBuilder UseTwitterDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}