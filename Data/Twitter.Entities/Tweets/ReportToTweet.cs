using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Tweets;

public class ReportToTweet : BaseEntity
{
    public string Text { get; set; } = string.Empty;

    public DateTime CloseDate { get; set; }

    public Guid ReasonId { get; set; }
    public virtual ReasonReport Reason { get; set; }

    public Guid TweetId { get; set; }
    public virtual Tweet Tweet { get; set; }

    public Guid CreatorId { get; set; }
    public virtual TwitterUser Creator { get; set; }
}