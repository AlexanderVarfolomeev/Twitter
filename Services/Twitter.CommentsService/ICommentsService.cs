using Twitter.CommentsService.Models;

namespace Twitter.CommentsService;

public interface ICommentsService
{
    CommentModel AddComment(CommentModelRequest modelRequest, Guid tweetId);
    IEnumerable<CommentModel> GetCommentsByTweet(Guid tweetId, int offset = 0, int limit = 10);
    void DeleteComment(Guid id);
    CommentModel UpdateComment(Guid id, CommentModelRequest modelRequest);
    IEnumerable<CommentModel> GetCommentsByUser(Guid userId);
}