using Serilog;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static class SerilogConfiguration
{
    public static void AddTwitterSerilog(this WebApplicationBuilder app)
    {
        app.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .Enrich.WithCorrelationId()
                .ReadFrom.Configuration(context.Configuration);
        });

        app.Services.AddHttpContextAccessor();
    }
}