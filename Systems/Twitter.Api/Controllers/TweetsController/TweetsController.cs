using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("get-first-{limit}-tweets")]
    public async Task<IEnumerable<TweetResponse>> GetTweets([FromRoute] int limit = 100)
    {
        var tweets = await _tweetsService.GetTweets(limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [HttpGet("get-first-{limit}-tweets-subscription")]
    public async Task<IEnumerable<TweetResponse>> GetTweetsBySubscription([FromRoute] int limit = 100)
    {
        var tweets = await _tweetsService.GetTweetsBySubscribes(limit);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }
    
    [HttpGet("get-tweets-by-userId:{userId}")]
    public async Task<IEnumerable<TweetResponse>> GetTweetsByUserId([FromRoute] Guid userId)
    {
        var tweets = await _tweetsService.GetTweetsByUserId(userId);
        return tweets.Select(x => _mapper.Map<TweetResponse>(x));
    }

    [HttpGet("{id}")]
    public async Task<TweetResponse> GetTweetById([FromRoute] Guid id)
    {
        var tweet = await _tweetsService.GetTweetById(id);
        return _mapper.Map<TweetResponse>(tweet);
    }

    [HttpPut("{id}")]
    public async Task<TweetResponse> UpdateTweet([FromRoute] Guid id, [FromBody] TweetRequest account)
    {
        var model = _mapper.Map<TweetModelRequest>(account);
        return _mapper.Map<TweetResponse>(await _tweetsService.UpdateTweet(id, model));
    }

    [HttpPost("")]
    public async Task<TweetResponse> AddTweet([FromBody] TweetRequest tweet)
    {
        var result = await _tweetsService.AddTweet(_mapper.Map<TweetModelRequest>(tweet));
        return _mapper.Map<TweetResponse>(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTweet([FromRoute] Guid id)
    {
        _tweetsService.DeleteTweet(id);
        return Ok();
    }

    [HttpPost("Like-{id}")]
    public Task<IActionResult> LikeTweet([FromRoute] Guid id)
    {
        _tweetsService.LikeTweet(id);
        return Task.FromResult<IActionResult>(Ok());
    }
}