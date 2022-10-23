using Serilog;

namespace Twitter.Api.Configuration.ApplicationExtensions;

public static partial class SerilogConfiguration
{
    public static IApplicationBuilder UseTwitterSerilog(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        return app;
    }
}