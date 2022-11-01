using AutoMapper;
using Twitter.Entities.Tweets;

namespace Twitter.TweetsService.Models;

public class TweetModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = String.Empty;
    
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}

public class TweetModelProfile : Profile
{
    public TweetModelProfile()
    {
        CreateMap<Tweet, TweetModel>();
    }
}