using Twitter.Context.Setup;

namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class DbConfiguration
{
    public static IApplicationBuilder UseTwitterDbContext(this WebApplication app)
    {
        DbInit.Execute(app.Services);
        DbSeed.Execute(app.Services);
        return app;
    }
}