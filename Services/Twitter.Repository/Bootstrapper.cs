using Microsoft.Extensions.DependencyInjection;
using Twitter.Entities.Base;
using Twitter.Settings.Interfaces;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

namespace Twitter.Repository;

public static class Bootstrapper
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}