using Microsoft.AspNetCore.Identity;
using Shared.Enum;
using Twitter.Entities.Base;

namespace Twitter.Entities.Users;

public class TwitterRole : IdentityRole<Guid>, IBaseEntity
{
    public TwitterPermissions Permissions { get; set; }

    //Name, Id - Определены в identityRole
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public virtual ICollection<TwitterUser> Users { get; set; }

    public bool IsNew
    {
        get => Id == Guid.Empty;
    }

    public void Init()
    {
        Id = Guid.NewGuid();
        CreationTime = ModificationTime = DateTime.UtcNow;
    }
}
