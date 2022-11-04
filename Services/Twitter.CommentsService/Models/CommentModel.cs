using AutoMapper;
using Twitter.Entities.Comments;

namespace Twitter.CommentsService.Models;

public class CommentModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;
    
    public Guid CreatorId { get; set; }
    public Guid TweetId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}

public class CommentModelProfile : Profile
{
    public CommentModelProfile()
    {
        CreateMap<Comment, CommentModel>();
    }
}