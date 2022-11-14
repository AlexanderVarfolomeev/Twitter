namespace Twitter.Api.Configuration.ServicesExtensions;

public static partial class SignalRConfiguration
{
    public static IServiceCollection AddTwitterSignalR(this IServiceCollection services)
    {
        services.AddSignalR();
        
      //  services.AddSingleton<ChatManager>();
        
        return services;
    }
    
}