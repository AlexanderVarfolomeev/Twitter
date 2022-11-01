using Twitter.Entities.Base;
using Twitter.Entities.Tweets;

namespace Twitter.Entities.Comments;

public class FileComment : BaseEntity
{
    public Guid CommentId { get; set; }
    public virtual Comment Comment { get; set; }
    
    public Guid FileId { get; set; }
    public virtual TwitterFile File { get; set; }
}