using Twitter.Entities.Base;

namespace Twitter.Entities.Messenger;

public class MessageFile : BaseEntity
{
    public Guid MessageId { get; set; }
    public virtual Message Message { get; set; }

    public Guid FileId { get; set; }
    public virtual TwitterFile File { get; set; }
}