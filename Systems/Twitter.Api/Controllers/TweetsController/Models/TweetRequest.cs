using AutoMapper;
using Twitter.TweetsService.Models;

namespace Twitter.Api.Controllers.TweetsController.Models;

public class TweetRequest
{
    public string Text { get; set; } = string.Empty;
}

public class TweetRequestProfile : Profile
{
    public TweetRequestProfile()
    {
        CreateMap<TweetRequest, TweetModelRequest>();
    }
}