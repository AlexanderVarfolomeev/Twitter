using Twitter.Entities.Comments;
using Twitter.Entities.Messenger;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;

namespace Twitter.Entities.Base;

public class TwitterFile : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public virtual ICollection<FileTweet> Tweets { get; set; }

    public virtual ICollection<FileComment> Comments { get; set; }

    public virtual ICollection<TwitterUser> Users { get; set; }

    public virtual ICollection<MessageFile> Messages { get; set; }
}