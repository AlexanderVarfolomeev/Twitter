using System;

namespace Twitter.WPF.Services.CommentsService.Models;

public class CommentResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public Guid TweetId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}