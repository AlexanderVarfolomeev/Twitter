using Microsoft.Extensions.DependencyInjection;

namespace Twitter.FileService;

public static class Bootstrapper
{
    public static IServiceCollection AddFileService(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
        return services;
    }
}