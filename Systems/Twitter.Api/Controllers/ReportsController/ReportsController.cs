using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.Api.Controllers.ReportsController.Models;
using Twitter.ReportServices;
using Twitter.ReportServices.Models;

namespace Twitter.Api.Controllers.ReportsController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    private readonly IMapper _mapper;

    public ReportsController(IReportService reportService, IMapper mapper)
    {
        _reportService = reportService;
        _mapper = mapper;
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-to-tweets")]
    public IEnumerable<ReportResponse> GetReportsToTweets([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _reportService.GetReportsToTweets(offset, limit)
            .Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-to-comments")]
    public IEnumerable<ReportResponse> GetReportsToComments([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _reportService.GetReportsToComments(offset, limit)
            .Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-by-tweet")]
    public IEnumerable<ReportResponse> GetReportsByTweet([FromQuery] Guid id, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _reportService.GetReportsByTweet(id, offset, limit).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-by-comment")]
    public IEnumerable<ReportResponse> GetReportsByComment([FromQuery] Guid id, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        return _reportService.GetReportsByComment(id, offset, limit).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("post-report-to-tweet")]
    public ReportResponse AddReportToTweet([FromQuery] Guid tweetId, [FromBody] ReportRequest report)
    {
        var result = _reportService.AddTweetReport(_mapper.Map<ReportModelRequest>(report), tweetId);
        return _mapper.Map<ReportResponse>(result);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("post-report-to-comment")]
    public ReportResponse AddReportToComment([FromQuery] Guid commentId, [FromBody] ReportRequest report)
    {
        var result = _reportService.AddCommentReport(_mapper.Map<ReportModelRequest>(report), commentId);
        return _mapper.Map<ReportResponse>(result);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("close-report-to-comment")]
    public IActionResult CloseReportToComment([FromQuery] Guid id)
    {
        _reportService.CloseReportToComment(id);
        return Ok();
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("close-report-to-tweet")]
    public IActionResult CloseReportToTweet([FromQuery] Guid id)
    {
        _reportService.CloseReportToTweet(id);
        return Ok();
    }
}