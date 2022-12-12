using Twitter.MessageService.Models;

namespace Twitter.MessageService;

public interface IMessageService
{
    IEnumerable<MessageModel> GetMessagesByDialog(Guid dialogId, int offset, int limit);
    IEnumerable<MessageModel> GetMessagesByUser(Guid userId, int offset = 0, int limit = 10);
    MessageModel SendMessage(MessageAddModelRequest addModelRequest, Guid userId);
}