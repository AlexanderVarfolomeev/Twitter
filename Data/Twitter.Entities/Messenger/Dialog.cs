using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Messenger;

public class Dialog : BaseEntity
{
    public virtual ICollection<UserDialog> UserDialogs { get; set; } 
    public virtual ICollection<Message> Messages { get; set; }
}