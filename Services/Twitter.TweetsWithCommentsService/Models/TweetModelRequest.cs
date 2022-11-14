using AutoMapper;
using Microsoft.AspNetCore.Http;
using Twitter.Entities.Tweets;

namespace TweetService.Models;

public class TweetModelRequest
{
    public string Text { get; set; } = string.Empty;

    public IEnumerable<IFormFile> Files { get; set; }
}

public class TweetModelRequestProfile : Profile
{
    public TweetModelRequestProfile()
    {
        CreateMap<TweetModelRequest, Tweet>();
    }
}