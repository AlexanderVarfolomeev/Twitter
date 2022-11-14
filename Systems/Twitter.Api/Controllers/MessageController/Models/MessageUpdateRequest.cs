namespace Twitter.Api.Controllers.MessageController.Models;

public class MessageUpdateRequest
{
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }
}