using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Messenger;

public class Message : BaseEntity
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }

    public Guid SenderId { get; set; }
    public virtual TwitterUser Sender { get; set; }

    public Guid DialogId { get; set; }
    public virtual Dialog Dialog { get; set; }

    public virtual ICollection<MessageFile> Files { get; set; }
}