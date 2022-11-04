using AutoMapper;
using Twitter.CommentsService.Models;

namespace Twitter.Api.Controllers.CommentsController.Models;

public class CommentRequest
{
    public string Text { get; set; } = string.Empty;
}

public class CommentRequestProfile : Profile
{
    public CommentRequestProfile()
    {
        CreateMap<CommentRequest, CommentModelRequest>();
    }
}