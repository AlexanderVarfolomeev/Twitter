namespace Twitter.DuendeServer.Configuration;

public static class DuendeConfiguration
{
    public static IServiceCollection AddDuende(this IServiceCollection services)
    {
        services.AddIdentityServer(options =>
        {
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryApiScopes(DuendeConfig.Scopes)
            .AddInMemoryIdentityResources(DuendeConfig.Resources)
            .AddInMemoryClients(DuendeConfig.Clients)
            .AddTestUsers(DuendeConfig.Users);

        return services;
    }

    public static WebApplication UseDuende(this WebApplication app)
    {
        app.UseIdentityServer();
        return app;
    }
}