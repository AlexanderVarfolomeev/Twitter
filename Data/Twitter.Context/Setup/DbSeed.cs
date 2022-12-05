using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Enum;
using Twitter.Context.Context;
using Twitter.Entities.Users;

namespace Twitter.Context.Setup;

public class DbSeed
{
    public static void Execute(IServiceProvider service)
    {
        using var scope = service.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        using var context = scope.ServiceProvider.GetRequiredService<MainDbContext>();
        var manager = scope.ServiceProvider.GetRequiredService<UserManager<TwitterUser>>();

        AddRolesAndAdmin(context, manager);
    }

    private static void AddRolesAndAdmin(MainDbContext context, UserManager<TwitterUser> manager)
    {
        if (!context.Roles.Any(x => x.Permissions == TwitterPermissions.Admin)
            || !context.Roles.Any(x => x.Permissions == TwitterPermissions.User)
            || !context.Roles.Any(x => x.Permissions == TwitterPermissions.FullAccessAdmin))
        {
            context.Roles.Add(new TwitterRole()
            {
                Name = "User",
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                Permissions = TwitterPermissions.User
            });
            
            context.Roles.Add(new TwitterRole()
            {
                Name = "Admin",
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                Permissions = TwitterPermissions.Admin
            });
            
            context.Roles.Add(new TwitterRole()
            {
                Name = "FullAdmin",
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                Permissions = TwitterPermissions.FullAccessAdmin
            });

            context.SaveChanges();
        }

        var user = new TwitterUser()
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Surname = "Admin",
            Birthday = DateTime.Now,
            CreationTime = DateTime.Now,
            ModificationTime = DateTime.Now,
            UserName = "Admin",
        };
        manager.CreateAsync(user, "pass");

        context.SaveChanges();

        context.TwitterRolesTwitterUsers.Add(new TwitterRoleTwitterUser()
        {
            RoleId = context.Roles.First(x => x.Permissions == TwitterPermissions.FullAccessAdmin).Id,
            UserId = user.Id,
            CreationTime = DateTime.Now,
            ModificationTime = DateTime.Now
        });

        context.SaveChanges();
    }
}