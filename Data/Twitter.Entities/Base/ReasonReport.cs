using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;

namespace Twitter.Entities.Base;

public class ReasonReport : BaseEntity
{
    public string Name { get; set; }
    
    public virtual ICollection<ReportToTweet> Tweets { get; set; }
    public virtual ICollection<ReportToComment> Comments { get; set; }
}