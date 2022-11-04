using AutoMapper;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;

namespace Twitter.ReportServices.Models;

public class ReportModel
{
    public string Text { get; set; } 

    public DateTime CloseDate { get; set; }

    public Guid ReasonId { get; set; }
    
    public Guid TweetOrCommentId { get; set; }

    public Guid CreatorId { get; set; }
}

public class ReportModelProfile : Profile
{
    public ReportModelProfile()
    {
        CreateMap<ReportToComment, ReportModel>()
            .ForMember(x => x.TweetOrCommentId, opt => opt.MapFrom(x => x.CommentId));
        CreateMap<ReportToTweet, ReportModel>()
            .ForMember(x => x.TweetOrCommentId, opt => opt.MapFrom(x => x.TweetId));
    }
}