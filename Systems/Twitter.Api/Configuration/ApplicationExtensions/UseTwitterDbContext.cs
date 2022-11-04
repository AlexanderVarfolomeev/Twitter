namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class DbConfiguration
{
    public static IApplicationBuilder UseTwitterDbContext(this WebApplication app)
    {
        return app;
    }
}