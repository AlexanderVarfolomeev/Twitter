using AutoMapper;
using FluentValidation;
using Twitter.Entities.Comments;

namespace Twitter.CommentsService.Models;

public class CommentModelRequest
{
    public string Text { get; set; } = string.Empty;
}

public class CommentModelRequestValidator : AbstractValidator<CommentModelRequest>
{
    public CommentModelRequestValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(140)
            .WithMessage("Maximum length is 140!");
    }
}

public class CommentModelRequestProfile : Profile
{
    public CommentModelRequestProfile()
    {
        CreateMap<CommentModelRequest, Comment>();
    }
}