namespace Twitter.Api.Configuration.ApplicationExtensions;

public static partial class CorsConfiguration
{
    public static void UseTwitterCors(this IApplicationBuilder app)
    {
        app.UseCors();
    }
}