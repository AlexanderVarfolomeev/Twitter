using Microsoft.AspNetCore.Identity;

namespace Twitter.Entities.Auth;

public class TwitterRole : IdentityRole<Guid>
{
    public TwitterPermissions Permissions { get; set; }

    //Name, Id - Определены в identityRole
}