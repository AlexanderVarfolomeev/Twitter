using Twitter.AccountService;
using Twitter.FileService;
using Twitter.Repository;
using Twitter.RoleService;
using Twitter.Settings;

namespace Twitter.Api;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddRepository();
        services.AddFileService();
        services.AddRoleService();
        services.AddAccountService();

        return services;
    }
}