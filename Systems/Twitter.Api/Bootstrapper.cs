using Twitter.AccountService;
using Twitter.CommentsService;
using Twitter.FileService;
using Twitter.ReportServices;
using Twitter.Repository;
using Twitter.RoleService;
using Twitter.Settings;
using Twitter.TweetsService;

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
        services.AddTweetsService();
        services.AddCommentsService();
        services.AddReportService();
        return services;
    }
}