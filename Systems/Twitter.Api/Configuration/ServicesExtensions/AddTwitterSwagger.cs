using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Twitter.Api.Configuration.ServicesExtensions;

public static partial class SwaggerConfiguration
{
    public static IServiceCollection AddTwitterSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                opts.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Version = description.GroupName,
                    Title = $"Twitter api"
                });
            }

            opts.ResolveConflictingActions(apiDesc => apiDesc.First());

            var xmlFile = $"api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            opts.IncludeXmlComments(xmlPath);

            opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Name = IdentityServerAuthenticationDefaults.AuthenticationScheme,
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Flows = new OpenApiOAuthFlows()
                {
                    Password = new OpenApiOAuthFlow()
                    {
                        TokenUrl = new Uri("https://localhost:5001/connect/token"), //TODO заменить на настройки
                        Scopes = new Dictionary<string, string>
                        {
                            {"twitter_api", "TwitterScope"}
                        }
                    },
                    ClientCredentials = new OpenApiOAuthFlow()
                    {
                        TokenUrl = new Uri("https://localhost:5001/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {"twitter_api", "TwitterScope"}
                        }
                    }
                }
            });

            opts.AddSecurityRequirement(new OpenApiSecurityRequirement()
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