using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;
using Twitter.Entities.Messenger;
using Twitter.Entities.Users;
using Twitter.FileService;
using Twitter.MessageService.Models;
using Twitter.Repository;

namespace Twitter.MessageService;

public class MessageService : IMessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<TwitterUser> _userRepository;
    private readonly IRepository<Dialog> _dialogRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<UserDialog> _usersDialogsRepository;

    private Guid _currentUserId;

    public MessageService(IRepository<Message> messageRepository, IRepository<TwitterUser> userRepository, 
        IRepository<Dialog> dialogRepository, IMapper mapper, IHttpContextAccessor accessor, IRepository<UserDialog> usersDialogsRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _dialogRepository = dialogRepository;
        _mapper = mapper;
        _usersDialogsRepository = usersDialogsRepository;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }
    
    public IEnumerable<MessageModel> GetMessagesByDialog(Guid dialogId, int offset = 0, int limit = 10)
    {
        var dialog = _dialogRepository.GetById(dialogId);
        
        ProcessException.ThrowIf(() => dialog.UserDialogs.All(x => x.UserId != _currentUserId), ErrorMessage.AccessRightsError);

        var messages = dialog.Messages
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)))
            .Select(x => _mapper.Map<MessageModel>(x))
            .OrderBy(x => x.CreationTime);

        return messages;
    }

    public IEnumerable<MessageModel> GetMessagesByUser(Guid userId, int offset, int limit)
    {
        var dialogs = _usersDialogsRepository.GetAll(x => x.UserId == _currentUserId).Select(x => x.DialogId);
        var dialog = _usersDialogsRepository.GetAll(x => dialogs.Contains(x.DialogId))
            .FirstOrDefault(x => x.UserId == userId);
        
        ProcessException.ThrowIf(() => dialog is null, ErrorMessage.NotFoundError); // диалога еще нет
        return GetMessagesByDialog(dialog.DialogId, offset, limit);
    }

    public MessageModel SendMessage(MessageAddModelRequest addModelRequest, Guid userId)
    {
        var message = _mapper.Map<Message>(addModelRequest);
        message.SenderId = _currentUserId;
        message.IsEdited = false;
        message.IsRead = false;

        var dialogs = _usersDialogsRepository.GetAll(x => x.UserId == _currentUserId).Select(x => x.DialogId);
        var dialog = _usersDialogsRepository.GetAll(x => dialogs.Contains(x.DialogId))
            .FirstOrDefault(x => x.UserId == userId);

        if (dialog is null)
        {
            Dialog newDialog = new Dialog();
            _dialogRepository.Save(newDialog);

            UserDialog userDialog1 = new UserDialog()
            {
                DialogId = newDialog.Id,
                UserId = _currentUserId
            };
            
            UserDialog userDialog2 = new UserDialog()
            {
                DialogId = newDialog.Id,
                UserId = userId
            };

            _usersDialogsRepository.Save(userDialog1);
            _usersDialogsRepository.Save(userDialog2);
        }

        dialog = _usersDialogsRepository.GetAll(x => dialogs.Contains(x.DialogId))
            .FirstOrDefault(x => x.UserId == userId);

        message.DialogId = dialog.DialogId;
        _messageRepository.Save(message);
        return _mapper.Map<MessageModel>(_messageRepository.Save(message));
    }
}