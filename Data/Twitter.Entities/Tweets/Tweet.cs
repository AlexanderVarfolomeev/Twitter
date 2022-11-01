using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Users;

namespace Twitter.Entities.Tweets;

public class Tweet : BaseEntity
{
    public string Text { get; set; } = String.Empty;
    
    public Guid CreatorId { get; set; }
    public virtual TwitterUser Creator { get; set; }
    
    public virtual ICollection<Comment> Comments { get; set; }

    public virtual ICollection<ReportToTweet> Reports { get; set; }
    
    public virtual ICollection<FileTweet> Files { get; set; }
    
    public virtual ICollection<UserLikeTweet> Likes { get; set; }
}