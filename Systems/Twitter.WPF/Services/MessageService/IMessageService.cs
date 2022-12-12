using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.WPF.Services.MessageService.Models;

namespace Twitter.WPF.Services.MessageService;

public interface IMessageService
{
    Task<IEnumerable<MessageView>> GetMessagesByUser(string userId, int offset = 0, int limit = 10);
}