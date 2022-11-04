using Twitter.CommentsService.Models;

namespace Twitter.CommentsService;

public interface ICommentsService
{
    Task<CommentModel> AddComment(CommentModelRequest modelRequest, Guid tweetId);
    Task<IEnumerable<CommentModel>> GetCommentsByTweet(Guid tweetId);
    Task DeleteComment(Guid id);
    Task<CommentModel> UpdateComment(Guid id, CommentModelRequest modelRequest);
    Task<IEnumerable<CommentModel>> GetCommentsByUser(Guid userId);

}