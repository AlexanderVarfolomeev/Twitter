using AutoMapper;
using Microsoft.AspNetCore.Http;
using Twitter.Entities.Messenger;

namespace Twitter.MessageService.Models;

public class MessageAddModelRequest
{
    public string Text { get; set; }
}

public class MessageAddModelRequestProfile : Profile
{
    public MessageAddModelRequestProfile()
    {
        CreateMap<MessageAddModelRequest, Message>();
    }
}