using System;
using Twitter.WPF.Services.AccountService.Models;

namespace Twitter.WPF.Services.MessageService.Models;

public class MessageView
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    
    public Guid SenderId { get; set; }

    public AccountModel Sender { get; set; }

}