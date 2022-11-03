using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.Repository;
using Twitter.TweetsService.Models;

namespace Twitter.TweetsService;

public class TweetsService : ITweetsService
{
    private readonly IRepository<Tweet> _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<TwitterUser> _userManager;
    private readonly IRepository<UserLikeTweet> _userLikeTweets;

    private readonly Guid _userId;
    
    public TweetsService(IHttpContextAccessor accessor, IRepository<Tweet> repository, IMapper mapper, UserManager<TwitterUser> userManager, IRepository<UserLikeTweet> userLikeTweets)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _userLikeTweets = userLikeTweets;

        _userId = Guid.Parse(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }
    
    public async Task<IEnumerable<TweetModel>> GetTweets(int limit = 100)
    {
        var tweets = _repository.GetAll().Take(limit);
        var result = (await tweets.ToListAsync()).Select(x => _mapper.Map<TweetModel>(x));
        return result;
    }

    public async Task<IEnumerable<TweetModel>> GetTweetsByUserId(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        ProcessException.ThrowIf(() => user is null, "The user with this id, doesn't exists.");
        return _repository.GetAll((x) => x.CreatorId == userId).Select(x => _mapper.Map<TweetModel>(x));
    }

    public Task<TweetModel> GetTweetById(Guid id)
    {
        var tweet = _repository.GetById(id);
        return Task.FromResult(_mapper.Map<TweetModel>(tweet));
    }

    public Task DeleteTweet(Guid id)
    {
        //var user = _userManager.FindByIdAsync(userId.ToString());
        //TODO проверка на роль юзера
        //ProcessException.ThrowIf();

        _repository.Delete(_repository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<TweetModel> AddTweet(TweetModelRequest requestModel)
    {
        var tweet = _mapper.Map<Tweet>(requestModel);
        tweet.CreatorId = _userId;
        return Task.FromResult(_mapper.Map<TweetModel>(_repository.Save(tweet)));
    }

    public Task<TweetModel> UpdateTweet(Guid id, TweetModelRequest requestModel)
    {
        var model = _repository.GetById(id);
        var tweet = _mapper.Map(requestModel, model);
        
        ProcessException.ThrowIf(() => tweet.CreatorId != _userId, "Only the person who created it can change a tweet!");
        
        return Task.FromResult(_mapper.Map<TweetModel>(_repository.Save(tweet)));
    }

    public Task LikeTweet(Guid idTweet)
    {
        var tweets = _userLikeTweets.GetAll(x => x.TweetId == idTweet && x.UserId == _userId);
        
        // Если юзер уже лайкал твит, то убираем лайк
        if (tweets.Any())
        {
            _userLikeTweets.Delete(tweets.First());
        }
        else
        {
            _userLikeTweets.Save(new UserLikeTweet() {TweetId = idTweet, UserId = _userId});
        }
        return Task.CompletedTask;
    }
}