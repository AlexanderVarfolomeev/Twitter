using AutoMapper;
using Twitter.MessageService.Models;

namespace Twitter.Api.Controllers.MessageController.Models;

public class MessageAddRequest
{
    public string Text { get; set; }
}

public class MessageAddRequestProfile : Profile
{
    public MessageAddRequestProfile()
    {
        CreateMap<MessageAddRequest, MessageAddModelRequest>();
    }
}