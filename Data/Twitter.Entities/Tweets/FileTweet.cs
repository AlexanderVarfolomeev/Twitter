using Twitter.Entities.Base;

namespace Twitter.Entities.Tweets;

public class FileTweet : BaseEntity
{
    public Guid TweetId { get; set; }
    public virtual Tweet Tweet { get; set; }
    
    public Guid FileId { get; set; }
    public virtual TwitterFile File { get; set; }
}