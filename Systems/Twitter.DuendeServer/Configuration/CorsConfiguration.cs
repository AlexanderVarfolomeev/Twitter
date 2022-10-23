namespace Twitter.DuendeServer.Configuration;

public static class CorsConfiguration
{
    public static IServiceCollection AddTwitterCors(this IServiceCollection services)
    {
        services.AddCors(builder =>
        {
            builder.AddDefaultPolicy(pol =>
            {
                pol.AllowAnyHeader();
                pol.AllowAnyMethod();
                pol.AllowAnyOrigin();
            });
        });

        return services;
    }

    public static void UseTwitterCors(this IApplicationBuilder app)
    {
        app.UseCors();
    }
}