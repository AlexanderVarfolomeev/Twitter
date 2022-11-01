using Twitter.AccountService;
using Twitter.FileService;
using Twitter.Repository;
using Twitter.Settings;

namespace Twitter.Api;

public static class Bootstrapper
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSettings();
        services.AddAccountService();
        services.AddRepository();
        services.AddFileService();
        return services;
    }
}