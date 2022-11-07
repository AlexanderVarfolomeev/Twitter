using AutoMapper;
using FluentValidation;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;

namespace Twitter.ReportServices.Models;

public class ReportModelRequest
{
    public string Text { get; set; }

    public Guid ReasonId { get; set; }
}

public class ReportModelRequestValidator : AbstractValidator<ReportModelRequest>
{
    public ReportModelRequestValidator()
    {
        RuleFor(x => x.ReasonId)
            .NotEmpty()
            .WithMessage("Please chose reason report.");
    }
}


public class ReportModelRequestProfile : Profile
{
    public ReportModelRequestProfile()
    {
        CreateMap<ReportModelRequest, ReportToComment>();
        CreateMap<ReportModelRequest, ReportToTweet>();
    }
}