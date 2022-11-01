using Twitter.Api.Middlewares;

namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class MiddlewaresConfiguration
{
    public static IApplicationBuilder UseTwitterMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
        return app;
    }
}