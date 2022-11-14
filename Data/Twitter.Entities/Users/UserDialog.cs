using Twitter.Entities.Base;
using Twitter.Entities.Messenger;

namespace Twitter.Entities.Users;

public class UserDialog : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual TwitterUser User { get; set; }
    
    public Guid DialogId { get; set; }
    public virtual Dialog Dialog { get; set; }
    public DateTime CreationTime { get; set; }
}