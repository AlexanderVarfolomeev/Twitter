using AutoMapper;
using Twitter.ReportServices.Models;

namespace Twitter.Api.Controllers.ReportsController.Models;

public class ReportResponse
{
    public Guid Id { get; set; }
    public string Text { get; set; } 

    public Guid ReasonId { get; set; }
    
    public Guid TweetOrCommentId { get; set; }

    public Guid CreatorId { get; set; }
}

public class ReportResponseProfile : Profile
{
    public ReportResponseProfile()
    {
        CreateMap<ReportModel, ReportResponse>();
    }
}
