using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Shared.Exceptions;
using Twitter.AccountService;
using Twitter.Api.Controllers.MessageController.Models;
using Twitter.Api.Hubs.Models;
using Twitter.MessageService;
using Twitter.MessageService.Models;

namespace Twitter.Api.Hubs.MessengerHub;

public class MessengerHub : Hub
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private static readonly ConnectionList ConnectionList = new();
    
    public MessengerHub(IAccountService accountService, IMapper mapper, IMessageService messageService)
    {
        _accountService = accountService;
        _mapper = mapper;
        _messageService = messageService;
    }
    
      
    public override async Task OnConnectedAsync()
    {
        var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var currentUserName = _accountService.GetAccountById(Guid.Parse(user)).UserName;
        var connectionId = Context.ConnectionId;
        
        ConnectionList.Add(currentUserName, connectionId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var user = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUserName = _accountService.GetAccountById(Guid.Parse(user)).UserName;
        var connectionId = Context.ConnectionId;
        
        ConnectionList.Remove(currentUserName, connectionId);
        await base.OnDisconnectedAsync(exception);
    }
    
    
    public async void SendMessage(MessageAddRequest message, Guid userId)
    {
        var senderId = Guid.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        ProcessException.ThrowIf(() => senderId == userId, "You can't send a message to yourself");
        
        var senderUserName = _accountService.GetAccountById(senderId).UserName;
        
        var userUserName = _accountService.GetAccountById(userId).UserName;
        
        var connections = new List<string>();
        
        foreach (var connection in ConnectionList.GetConnections(senderUserName))
        {
            connections.Add(connection);
        }
        
        foreach (var connection in ConnectionList.GetConnections(userUserName))
        {
            connections.Add(connection);
        }

        var response =_mapper.Map<MessageResponse>(_messageService.SendMessage(_mapper.Map<MessageAddModelRequest>(message), userId));
        await Clients.Clients(connections).SendAsync("ReceiveMessage", response); // При получении на клиенте мы сразу вызовем запрос на получение картинок
    }
    
    
}