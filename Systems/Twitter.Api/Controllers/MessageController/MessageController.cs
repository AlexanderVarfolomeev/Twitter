using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.MessageController.Models;
using Twitter.MessageService;
using Twitter.MessageService.Models;

namespace Twitter.Api.Controllers.MessageController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;

    public MessageController(IMessageService messageService, IMapper mapper)
    {
        _messageService = messageService;
        _mapper = mapper;
    }

    [HttpGet("get-by-dialog")]
    public IEnumerable<MessageResponse> GetMessagesByDialog([FromQuery] Guid dialogId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
       var messages = ( _messageService.GetMessagesByDialog(dialogId, offset, limit))
           .Select(x => _mapper.Map<MessageResponse>(x));
       return messages;
    }
    
    [HttpGet("get-by-user")]
    public IEnumerable<MessageResponse> GetMessagesByUser([FromQuery] Guid userId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var messages = ( _messageService.GetMessagesByUser(userId, offset, limit))
            .Select(x => _mapper.Map<MessageResponse>(x));
        return messages;
    }

    [HttpPost("")]
    public MessageResponse SendMessage([FromBody] MessageAddRequest message, [FromQuery] Guid userId)
    {
        return _mapper.Map<MessageResponse>( _messageService.SendMessage(_mapper.Map<MessageAddModelRequest>(message), userId));
    }
}