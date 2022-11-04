namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class CorsConfiguration
{
    public static void UseTwitterCors(this IApplicationBuilder app)
    {
        app.UseCors();
    }
}