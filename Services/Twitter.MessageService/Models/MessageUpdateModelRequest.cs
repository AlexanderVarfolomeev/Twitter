using Microsoft.AspNetCore.Http;

namespace Twitter.MessageService.Models;

public class MessageUpdateModelRequest
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
}