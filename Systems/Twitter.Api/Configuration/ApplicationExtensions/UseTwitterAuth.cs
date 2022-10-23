﻿namespace Twitter.Api.Configuration.ApplicationExtensions;

public static partial class AuthConfiguration
{
   public static IApplicationBuilder UseTwitterAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}