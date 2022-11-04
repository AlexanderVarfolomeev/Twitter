using Twitter.Entities.Base;

namespace Twitter.Entities.Users;

public class TwitterRoleTwitterUser : BaseEntity
{
    public Guid RoleId { get; set; }
    public virtual TwitterRole Role { get; set; }

    public Guid UserId { get; set; }
    public virtual TwitterUser User { get; set; }
}