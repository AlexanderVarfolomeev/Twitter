using System;

namespace Twitter.WPF.Services.MessageService.Models;

public class MessageResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    
    public Guid SenderId { get; set; }

    public Guid DialogId { get; set; }
}