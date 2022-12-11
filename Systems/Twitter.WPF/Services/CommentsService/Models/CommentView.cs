using System;
using System.Collections.Generic;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.CommentsService.Models;

public class CommentView
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public Guid TweetId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public AccountModel Creator { get; set; }
    public List<string> Attachments { get; set; }
    public CommentView(CommentResponse comment)
    {
        Id = comment.Id;
        Text = comment.Text;
        CreatorId = comment.CreatorId;
        TweetId = comment.TweetId;
        CreationTime = comment.CreationTime;
        ModificationTime = comment.ModificationTime;
    }

    public CommentView()
    {
        
    }
}