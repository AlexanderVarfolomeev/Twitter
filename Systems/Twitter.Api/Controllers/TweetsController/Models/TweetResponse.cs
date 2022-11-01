using AutoMapper;
using Twitter.TweetsService.Models;

namespace Twitter.Api.Controllers.TweetsController.Models;

public class TweetResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; } = String.Empty;
    
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }   
}

public class TweetResponseProfile : Profile
{
    public TweetResponseProfile()
    {
        CreateMap<TweetModel, TweetResponse>();
    }
}

