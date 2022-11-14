using Microsoft.Extensions.DependencyInjection;

namespace Shared.Helpers;

public static class AutoMapperRegisterService
{
    public static void Register(IServiceCollection sevices)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(s => s.FullName != null && s.FullName.ToLower().StartsWith("twitter."));

        sevices.AddAutoMapper(assemblies);
    }
}