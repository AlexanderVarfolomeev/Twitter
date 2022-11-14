using Microsoft.Extensions.DependencyInjection;
using TweetService.Models;

namespace TweetService;

public static class Bootstrapper
{
    public static IServiceCollection AddTweetService(this IServiceCollection services)
    {
        services.AddScoped<ITweetsWithCommentsService, TweetsWithCommentsService>();
        return services;
    }
}