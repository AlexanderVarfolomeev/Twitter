using Twitter.MessageService.Models;

namespace Twitter.MessageService;

public interface IMessageService
{
    IEnumerable<MessageModel> GetMessages(Guid dialogId, int offset, int limit);
    MessageModel SendMessage(MessageAddModelRequest addModelRequest, Guid userId);
}