
using AutoMapper;
using Twitter.Entities.Messenger;

namespace Twitter.MessageService.Models;

public class MessageModel
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsRead { get; set; }
    public bool IsEdited { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    
    public Guid SenderId { get; set; }

    public Guid DialogId { get; set; }

}

public class MessageModelProfile : Profile
{
    public MessageModelProfile()
    {
        CreateMap<Message, MessageModel>();
    }
}