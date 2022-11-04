using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Messenger;

public class Dialog : BaseEntity
{
    public Guid FirstUserId { get; set; }
    public virtual TwitterUser FirstUser { get; set; }

    public Guid SecondUserId { get; set; }
    public virtual TwitterUser SecondUser { get; set; }

    public virtual ICollection<Message> Messages { get; set; }
}