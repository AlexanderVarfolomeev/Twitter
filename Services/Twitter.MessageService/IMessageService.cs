using Twitter.MessageService.Models;

namespace Twitter.MessageService;

public interface IMessageService
{
    Task<IEnumerable<MessageModel>> GetMessages(Guid dialogId, int offset, int limit);
    Task<MessageModel> SendMessage(MessageAddModelRequest addModelRequest, Guid userId);
}