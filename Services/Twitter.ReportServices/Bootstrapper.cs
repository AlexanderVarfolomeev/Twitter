using Microsoft.Extensions.DependencyInjection;
using Twitter.ReportServices.Models;

namespace Twitter.ReportServices;

public static class Bootstrapper
{
    public static IServiceCollection AddReportService(this IServiceCollection services)
    {
        services.AddScoped<IReportService, ReportService>();
        return services;
    }
    
}