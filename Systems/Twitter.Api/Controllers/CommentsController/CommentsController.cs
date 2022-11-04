using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.CommentsController.Models;
using Twitter.CommentsService;
using Twitter.CommentsService.Models;

namespace Twitter.Api.Controllers.CommentsController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICommentsService _commentsService;

    public CommentsController(IMapper mapper, ICommentsService commentsService)
    {
        _mapper = mapper;
        _commentsService = commentsService;
    }

    [HttpGet("get-comment-by-tweet-{tweetId}")]
    public async Task<IEnumerable<CommentResponse>> GetCommentsByTweet([FromRoute] Guid tweetId)
    {
        var comments = await _commentsService.GetCommentsByTweet(tweetId);
        return comments.Select(x => _mapper.Map<CommentResponse>(x));
    }
    
    [HttpGet("get-comment-by-user-{userId}")]
    public async Task<IEnumerable<CommentResponse>> GetCommentsByUser([FromRoute] Guid userId)
    {
        var comments = await _commentsService.GetCommentsByUser(userId);
        return comments.Select(x => _mapper.Map<CommentResponse>(x));
    }

    [HttpPost("{tweetId}")]
    public async Task<CommentResponse> AddComment([FromRoute] Guid tweetId, [FromBody] CommentRequest comment)
    {
        var commentModelResponse = await _commentsService.AddComment(_mapper.Map<CommentModelRequest>(comment), tweetId);
        return _mapper.Map<CommentResponse>(commentModelResponse);
    }

    [HttpPut("{commentId}")]
    public async Task<CommentResponse> UpdateComment([FromRoute] Guid commentId, [FromBody] CommentRequest comment)
    {
        var commentModelResponse = await _commentsService.UpdateComment( commentId, _mapper.Map<CommentModelRequest>(comment));
        return _mapper.Map<CommentResponse>(commentModelResponse); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
    {
        await _commentsService.DeleteComment(id);
        return Ok();
    }
    
}