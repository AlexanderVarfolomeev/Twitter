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

    [HttpGet("")]
    public async Task<IEnumerable<MessageResponse>> GetMessages([FromQuery] Guid dialogId, [FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
       var messages = (await _messageService.GetMessages(dialogId, offset, limit))
           .Select(x => _mapper.Map<MessageResponse>(x));
       return messages;
    }

    [HttpPost("")]
    public async Task<MessageResponse> SendMessage([FromBody] MessageAddRequest message, [FromQuery] Guid userId)
    {
      
        return _mapper.Map<MessageResponse>(await _messageService.SendMessage(_mapper.Map<MessageAddModelRequest>(message), userId));
    }
}