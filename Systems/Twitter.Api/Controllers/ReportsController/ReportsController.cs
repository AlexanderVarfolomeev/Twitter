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
    public async Task<IEnumerable<ReportResponse>> GetReportsToTweets()
    {
        return (await _reportService.GetReportsToTweets()).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-to-comments")]
    public async Task<IEnumerable<ReportResponse>> GetReportsToComments()
    {
        return (await _reportService.GetReportsToComments()).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-by-tweet-{id}")]
    public async Task<IEnumerable<ReportResponse>> GetReportsByTweet([FromRoute] Guid id)
    {
        return (await _reportService.GetReportsByTweet(id)).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-reports-by-comment-{id}")]
    public async Task<IEnumerable<ReportResponse>> GetReportsByComment([FromRoute] Guid id)
    {
        return (await _reportService.GetReportsByComment(id)).Select(x => _mapper.Map<ReportResponse>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("post-report-to-tweet-{tweetId}")]
    public async Task<ReportResponse> AddReportToTweet([FromRoute] Guid tweetId, [FromBody] ReportRequest report)
    {
        var result = await _reportService.AddTweetReport(_mapper.Map<ReportModelRequest>(report), tweetId);
        return _mapper.Map<ReportResponse>(result);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("post-report-to-comment-{commentId}")]
    public async Task<ReportResponse> AddReportToComment([FromRoute] Guid commentId, [FromBody] ReportRequest report)
    {
        var result = await _reportService.AddCommentReport(_mapper.Map<ReportModelRequest>(report), commentId);
        return _mapper.Map<ReportResponse>(result);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("close-report-to-comment-{id}")]
    public async Task<IActionResult> CloseReportToComment([FromRoute] Guid id)
    {
        await _reportService.CloseReportToComment(id);
        return Ok();
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("close-report-to-tweet-{id}")]
    public async Task<IActionResult> CloseReportToTweet([FromRoute] Guid id)
    {
        await _reportService.CloseReportToTweet(id);
        return Ok();
    }
}