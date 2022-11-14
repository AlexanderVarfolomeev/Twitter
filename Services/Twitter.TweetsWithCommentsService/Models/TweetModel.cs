using AutoMapper;
using Twitter.Entities.Tweets;

namespace TweetService.Models;

public class TweetModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = string.Empty;

    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }

    public int CountOfLikes { get; set; }

    public string CreatorUserName { get; set; }
    public IEnumerable<Guid> ImagesId { get; set; }
    public IEnumerable<CommentModel> Comments { get; set; }
}

public class TweetModelProfile : Profile
{
    public TweetModelProfile()
    {
        CreateMap<Tweet, TweetModel>()
            .ForMember(x => x.CountOfLikes, o => o.MapFrom(y => y.Likes.Count));
    }
}