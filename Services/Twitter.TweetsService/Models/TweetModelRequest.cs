using AutoMapper;
using Twitter.Entities.Tweets;

namespace Twitter.TweetsService.Models;

public class TweetModelRequest
{
    public string Text { get; set; } = string.Empty;
}

public class TweetModelRequestProfile : Profile
{
    public TweetModelRequestProfile()
    {
        CreateMap<TweetModelRequest, Tweet>();
    }
}