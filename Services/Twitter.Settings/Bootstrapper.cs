using Microsoft.Extensions.DependencyInjection;
using Twitter.Settings.Interfaces;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

namespace Twitter.Settings;

public static class Bootstrapper
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddSingleton<ISettingSource, SettingSource>();
        services.AddSingleton<IDbSettings, DbSettings>();
        return services;
    }
}