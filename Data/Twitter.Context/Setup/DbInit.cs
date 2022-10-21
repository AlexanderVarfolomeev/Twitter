﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Twitter.Context.Context;

namespace Twitter.Context.Setup;

public class DbInit
{
    public static void Execute(IServiceProvider service)
    {
        using var scope = service.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
        using var context = factory.CreateDbContext();

        context.Database.Migrate();
    }
}