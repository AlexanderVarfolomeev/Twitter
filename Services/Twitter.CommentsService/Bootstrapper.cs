using Microsoft.Extensions.DependencyInjection;

namespace Twitter.CommentsService;

public static class Bootstrapper
{
    public static IServiceCollection AddCommentsService(this IServiceCollection services)
    {
        services.AddScoped<ICommentsService, CommentService>();
        return services;
    }
    
}