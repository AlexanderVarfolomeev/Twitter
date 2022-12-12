using AutoMapper;
using Twitter.Entities.Messenger;
using Twitter.MessageService.Models;

namespace Twitter.Api.Controllers.MessageController.Models;

public class MessageResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    //public bool IsRead { get; set; }
    //public bool IsEdited { get; set; }

    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    
    public Guid SenderId { get; set; }

    public Guid DialogId { get; set; }

}

public class MessageResponseProfile : Profile
{
    public MessageResponseProfile()
    {
        CreateMap<MessageModel, MessageResponse>();
    }
}