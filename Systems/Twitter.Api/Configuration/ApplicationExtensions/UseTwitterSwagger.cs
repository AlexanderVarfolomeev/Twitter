using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Twitter.Api.Configuration.ApplicationExtensions;

public static class SwaggerConfiguration
{
    public static IApplicationBuilder UseTwitterSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(options => { });
        app.UseSwaggerUI(options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName);
            ;

            options.DocExpansion(DocExpansion.List);
            options.DefaultModelsExpandDepth(-1);
            options.OAuthAppName("Twitter_api");

            options.OAuthClientId("swagger");
            options.OAuthClientSecret("secret");
        });
        return app;
    }
}