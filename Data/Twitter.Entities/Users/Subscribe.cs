using Twitter.Entities.Base;

namespace Twitter.Entities.Users;

public class Subscribe : BaseEntity 
{
    public Guid SubscriberId { get; set; }
    public TwitterUser Subscriber { get; set; }
    
    public Guid UserId { get; set; }
    public TwitterUser User { get; set; }
}