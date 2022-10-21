using Microsoft.AspNetCore.Mvc;

namespace Twitter.Api.Configuration;

public static class VersioningConfiguration
{
    public static IServiceCollection AddTwitterVersions(this IServiceCollection services)
    {
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        
        services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            }
        );

        

        return services;
    }
}