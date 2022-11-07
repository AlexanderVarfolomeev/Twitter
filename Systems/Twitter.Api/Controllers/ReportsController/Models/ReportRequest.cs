using AutoMapper;
using FluentValidation;
using Twitter.ReportServices.Models;

namespace Twitter.Api.Controllers.ReportsController.Models;

public class ReportRequest
{
    public string Text { get; set; }

    public Guid ReasonId { get; set; }
}

public class ReportRequestValidator : AbstractValidator<ReportRequest>
{
    public ReportRequestValidator()
    {
        RuleFor(x => x.ReasonId)
            .NotEmpty()
            .WithMessage("Please chose reason report.");
    }
}

public class ReportRequestProfile : Profile
{
    public ReportRequestProfile()
    {
        CreateMap<ReportRequest, ReportModelRequest>();
    }
}