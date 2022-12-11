using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.WPF.Services.CommentsService.Models;

namespace Twitter.WPF.Services.CommentsService;

public interface ICommentsService
{
    Task<IEnumerable<CommentView>> GetCommentsByTweetId(string tweetId, int offset = 0, int limit = 10);
    Task AddComment(string tweetId, CommentRequest comment, List<string> attachments);
}