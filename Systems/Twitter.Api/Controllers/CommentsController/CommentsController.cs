using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.Api.Controllers.CommentsController.Models;
using Twitter.CommentsService;
using Twitter.CommentsService.Models;

namespace Twitter.Api.Controllers.CommentsController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentsService _commentsService;

    public CommentsController(IMapper mapper, ICommentsService commentsService)
    {
        _mapper = mapper;
        _commentsService = commentsService;
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-comment-by-tweet")]
    public IEnumerable<CommentResponse> GetCommentsByTweet([FromQuery] Guid tweetId, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var comments = _commentsService.GetCommentsByTweet(tweetId, offset, limit);
        return comments.Select(x => _mapper.Map<CommentResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-comment-by-user")]
    public IEnumerable<CommentResponse> GetCommentsByUser([FromQuery] Guid userId)
    {
        var comments = _commentsService.GetCommentsByUser(userId);
        return comments.Select(x => _mapper.Map<CommentResponse>(x));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("")]
    public CommentResponse AddComment([FromQuery] Guid tweetId, [FromBody] CommentRequest comment)
    {
        var commentModelResponse =
            _commentsService.AddComment(_mapper.Map<CommentModelRequest>(comment), tweetId);
        return _mapper.Map<CommentResponse>(commentModelResponse);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("")]
    public CommentResponse UpdateComment([FromQuery] Guid commentId, [FromBody] CommentRequest comment)
    {
        var commentModelResponse =
             _commentsService.UpdateComment(commentId, _mapper.Map<CommentModelRequest>(comment));
        return _mapper.Map<CommentResponse>(commentModelResponse);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpDelete("{id}")]
    public IActionResult DeleteComment([FromRoute] Guid id)
    {
        _commentsService.DeleteComment(id);
        return Ok();
    }
}