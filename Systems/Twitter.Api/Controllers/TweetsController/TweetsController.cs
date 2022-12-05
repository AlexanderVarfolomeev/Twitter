using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.Api.Controllers.TweetsController.Models;
using Twitter.TweetsService;
using Twitter.TweetsService.Models;

namespace Twitter.Api.Controllers.TweetsController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TweetsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITweetsService _tweetsService;

    public TweetsController(ITweetsService tweetsService, IMapper mapper)
    {
        _tweetsService = tweetsService;
        _mapper = mapper;
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-tweets")]
    public IEnumerable<TweetResponse> GetTweets([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var tweets = _tweetsService.GetTweets(offset, limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-tweets-subscription")]
    public IEnumerable<TweetResponse> GetTweetsBySubscription([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        var tweets = _tweetsService.GetTweetsBySubscribes(offset, limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-tweets-by-user:{userId}")]
    public IEnumerable<TweetResponse> GetTweetsByUserId([FromRoute] Guid userId, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var tweets = _tweetsService.GetTweetsByUserId(userId, offset, limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-tweets-for-last-days")]
    public IEnumerable<TweetResponse> GetTweetsForLastDays([FromRoute] int days, [FromQuery] int offset = 0,
        [FromQuery] int limit = 10)
    {
        var tweets = _tweetsService.GetTweetsForLastDays(days, offset, limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("{id}")]
    public TweetResponse GetTweetById([FromRoute] Guid id)
    {
        var tweet = _tweetsService.GetTweetById(id);
        return _mapper.Map<TweetResponse>(tweet);
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("likes-{id}")]
    public int GetCountLikesByTweet([FromRoute] Guid id)
    {
        return _tweetsService.GetCountLikesOfTweet(id);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("{id}")]
    public TweetResponse UpdateTweet([FromRoute] Guid id, [FromBody] TweetRequest account)
    {
        var model = _mapper.Map<TweetModelRequest>(account);
        return _mapper.Map<TweetResponse>(_tweetsService.UpdateTweet(id, model));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("")]
    public TweetResponse AddTweet([FromBody] TweetRequest tweet)
    {
        var result = _tweetsService.AddTweet(_mapper.Map<TweetModelRequest>(tweet));
        return _mapper.Map<TweetResponse>(result);
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpDelete("{id}")]
    public IActionResult DeleteTweet([FromRoute] Guid id)
    {
        _tweetsService.DeleteTweet(id);
        return Ok();
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("Like-{id}")]
    public IActionResult LikeTweet([FromRoute] Guid id)
    {
        _tweetsService.LikeTweet(id);
        return Ok();
    }
}