using Microsoft.Extensions.DependencyInjection;

namespace Twitter.Repository;

public static class Bootstrapper
{
    public static IServiceCollection AddRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}