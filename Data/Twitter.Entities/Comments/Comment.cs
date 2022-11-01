using Twitter.Entities.Base;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;

namespace Twitter.Entities.Comments;

public class Comment : BaseEntity
{
    public string Text { get; set; } = String.Empty;

    public Guid CreatorId { get; set; }
    public virtual TwitterUser Creator { get; set; }
        
    public Guid TweetId { get; set; }
    public virtual Tweet Tweet { get; set; }
    
    public virtual ICollection<ReportToComment> Reports { get; set; }
    
    public virtual ICollection<FileComment> Files { get; set; }
}