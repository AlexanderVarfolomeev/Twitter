using Twitter.Entities.Base;
using Twitter.Entities.Users;

namespace Twitter.Entities.Comments;

public class ReportToComment : BaseEntity
{
    public string Text { get; set; } = string.Empty;

    public DateTime CloseDate { get; set; }

    public Guid ReasonId { get; set; }
    public virtual ReasonReport Reason { get; set; }

    public Guid CommentId { get; set; }
    public virtual Comment Comment { get; set; }

    public Guid CreatorId { get; set; }
    public virtual TwitterUser Creator { get; set; }
}