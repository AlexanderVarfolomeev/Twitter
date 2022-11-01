using Microsoft.Extensions.DependencyInjection;

namespace Twitter.TweetsService;

public static class Bootstrapper
{
    public static IServiceCollection AddTweetsService(this IServiceCollection services)
    {
        services.AddScoped<ITweetsService, TweetsService>();
        return services;
    }
}