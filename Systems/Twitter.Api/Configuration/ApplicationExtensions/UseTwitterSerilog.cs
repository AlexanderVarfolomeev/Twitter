using Serilog;

namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class SerilogConfiguration
{
    public static IApplicationBuilder UseTwitterSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}