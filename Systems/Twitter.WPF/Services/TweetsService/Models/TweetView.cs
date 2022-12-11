using System;
using System.Collections.Generic;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.TweetsService.Models;

public class TweetView
{
    public string Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public AccountModel Creator { get; set; }
    
    public List<string> Attachments { get; set; }
    public int CountOfLikes { get; set; }
    public int CountOfComments { get; set; }
    public DateTime CreationTime { get; set; }

}