using AutoMapper;
using FluentValidation;
using Twitter.TweetsService.Models;

namespace Twitter.Api.Controllers.TweetsController.Models;

public class TweetRequest
{
    public string Text { get; set; } = string.Empty;
}

public class TweetRequestValidator : AbstractValidator<TweetRequest>
{
    public TweetRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Tweet cannot be without text!")
            .MaximumLength(140)
            .WithMessage("Maximum length is 140 chars!");
    }
}

public class TweetRequestProfile : Profile
{
    public TweetRequestProfile()
    {
        CreateMap<TweetRequest, TweetModelRequest>();
    }
}