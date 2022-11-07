using AutoMapper;
using FluentValidation;
using Twitter.CommentsService.Models;

namespace Twitter.Api.Controllers.CommentsController.Models;

public class CommentRequest
{
    public string Text { get; set; } = string.Empty;
}

public class CommentRequestValidator : AbstractValidator<CommentRequest>
{
    public CommentRequestValidator()
    {
        RuleFor(x => x.Text)
            .MaximumLength(140)
            .WithMessage("Maximum length is 140!");
    }
}

public class CommentRequestProfile : Profile
{
    public CommentRequestProfile()
    {
        CreateMap<CommentRequest, CommentModelRequest>();
    }
}