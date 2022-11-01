using Microsoft.Extensions.DependencyInjection;

namespace Twitter.RoleService;

public static class Bootstrapper
{
    public static IServiceCollection AddRoleService(this IServiceCollection services)
    {
        services.AddScoped<IRoleService, RoleService>();
        return services;
    }
}