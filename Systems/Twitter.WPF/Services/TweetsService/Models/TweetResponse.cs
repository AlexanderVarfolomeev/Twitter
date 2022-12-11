using System;
using System.Collections.Generic;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.TweetsService.Models;

public class TweetResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}