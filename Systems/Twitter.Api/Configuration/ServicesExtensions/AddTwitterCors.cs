namespace Twitter.Api.Configuration.ServicesExtensions;

public static partial class CorsConfiguration
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
}