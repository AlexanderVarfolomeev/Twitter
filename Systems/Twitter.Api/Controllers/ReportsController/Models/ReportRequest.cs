using AutoMapper;
using Twitter.ReportServices.Models;

namespace Twitter.Api.Controllers.ReportsController.Models;

public class ReportRequest
{
    public string Text { get; set; } 

    public Guid ReasonId { get; set; }
}

public class ReportRequestProfile : Profile
{
    public ReportRequestProfile()
    {
        CreateMap<ReportRequest, ReportModelRequest>();
    }
}
