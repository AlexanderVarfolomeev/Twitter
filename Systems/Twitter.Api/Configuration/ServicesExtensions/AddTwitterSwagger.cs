using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Shared.Security;
using Twitter.Settings.Interfaces;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddTwitterSwagger(this IServiceCollection services, ITwitterApiSettings apiSettings)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                opts.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Version = description.GroupName,
                    Title = "Twitter api"
                });

            opts.ResolveConflictingActions(apiDesc => apiDesc.First());

            var xmlFile = "api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opts.IncludeXmlComments(xmlPath);

            opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Name = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(apiSettings.Duende.Url + "/connect/token"), 
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.TwitterRead, "Twitter API read data."},
                            {AppScopes.TwitterWrite, "Twitter API write data."}
                        }
                    },
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(apiSettings.Duende.Url + "/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {AppScopes.TwitterRead, "Twitter API read data."}
                        }
                    }
                }
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"
                        }
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }
}