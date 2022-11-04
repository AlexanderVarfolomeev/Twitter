using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Tweets;

public class UserLikeTweet : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual TwitterUser User { get; set; }

    public Guid TweetId { get; set; }
    public virtual Tweet Tweet { get; set; }
}