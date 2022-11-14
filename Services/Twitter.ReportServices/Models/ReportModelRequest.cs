using AutoMapper;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;

namespace Twitter.ReportServices.Models;

public class ReportModelRequest
{
    public string Text { get; set; }

    public Guid ReasonId { get; set; }
}

public class ReportModelRequestProfile : Profile
{
    public ReportModelRequestProfile()
    {
        CreateMap<ReportModelRequest, ReportToComment>();
        CreateMap<ReportModelRequest, ReportToTweet>();
    }
}