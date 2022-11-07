using AutoMapper;
using FluentValidation;
using Twitter.Entities.Tweets;

namespace Twitter.TweetsService.Models;

public class TweetModelRequest
{
    public string Text { get; set; } = string.Empty;
}

public class TweetModelRequestValidator : AbstractValidator<TweetModelRequest>
{
    public TweetModelRequestValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Tweet cannot be without text!")
            .MaximumLength(140)
            .WithMessage("Maximum length is 140 chars!");
    }
}


public class TweetModelRequestProfile : Profile
{
    public TweetModelRequestProfile()
    {
        CreateMap<TweetModelRequest, Tweet>();
    }
}