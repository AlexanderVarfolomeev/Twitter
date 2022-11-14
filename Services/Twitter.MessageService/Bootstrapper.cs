using Microsoft.Extensions.DependencyInjection;

namespace Twitter.MessageService;

public static class Bootstrapper
{
    public static IServiceCollection AddMessageService(this IServiceCollection services)
    {
        services.AddScoped<IMessageService, MessageService>();
        return services;
    }
}