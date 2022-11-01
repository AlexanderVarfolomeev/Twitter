using Shared.Helpers;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static class AutomapperConfiguration
{
    public static IServiceCollection AddTwitterAutomapper(this IServiceCollection services)
    {
        AutoMapperRegisterService.Register(services);
        return services;
    }
}