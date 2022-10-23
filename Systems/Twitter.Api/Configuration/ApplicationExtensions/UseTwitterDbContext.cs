namespace Twitter.Api.Configuration.ApplicationExtensions;

public static partial class DbConfiguration
{
    public static IApplicationBuilder UseTwitterDbContext(this IApplicationBuilder app)
    {
        return app;
    }
}