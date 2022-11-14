using AutoMapper;
using Twitter.Entities.Comments;

namespace Twitter.CommentsService.Models;

public class CommentModelRequest
{
    public string Text { get; set; } = string.Empty;
}

public class CommentModelRequestProfile : Profile
{
    public CommentModelRequestProfile()
    {
        CreateMap<CommentModelRequest, Comment>();
    }
}