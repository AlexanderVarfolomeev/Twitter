using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.Repository;
using Twitter.TweetsService.Models;

namespace Twitter.TweetsService;

public class TweetsService : ITweetsService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TwitterUser> _usersRepository;
    private readonly IRepository<Tweet> _tweetRepository;

    private readonly Guid _currentUserId;
    private readonly IRepository<UserLikeTweet> _userLikeTweetsRepository;
    private readonly IRepository<TwitterRoleTwitterUser> _rolesUserRepository;

    public TweetsService(IHttpContextAccessor accessor, IRepository<Tweet> tweetRepository, IMapper mapper,
        IRepository<TwitterUser> usersRepository, IRepository<UserLikeTweet> userLikeTweetsRepository,IRepository<TwitterRoleTwitterUser> rolesUserRepository)
    {
        _tweetRepository = tweetRepository;
        _mapper = mapper;
        _usersRepository = usersRepository;
        _userLikeTweetsRepository = userLikeTweetsRepository;
        _rolesUserRepository = rolesUserRepository;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }

    public async Task<IEnumerable<TweetModel>> GetTweets(int limit = 100)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty &&  IsBanned(_currentUserId), "You are banned!");
        
        var tweets = _tweetRepository.GetAll().Take(limit);
        var result = (await tweets.ToListAsync()).Select(x => _mapper.Map<TweetModel>(x));
        return result;
    }

    public async Task<IEnumerable<TweetModel>> GetTweetsBySubscribes(int limit = 100)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty &&  IsBanned(_currentUserId), "You are banned!");
        var subscribes = _usersRepository.GetById(_currentUserId).Subscribes.Select(x => x.UserId);

        var tweets = _tweetRepository.GetAll(x => subscribes.Contains(x.CreatorId));
        var result = (await tweets.ToListAsync()).Select(x => _mapper.Map<TweetModel>(x)).OrderByDescending(x => x.CreationTime);
        
        return result;
    }

    public Task<IEnumerable<TweetModel>> GetTweetsByUserId(Guid userId)
    {
        ProcessException.ThrowIf(() =>_currentUserId != Guid.Empty &&  IsBanned(_currentUserId), "You are banned!");
        
        return Task.FromResult<IEnumerable<TweetModel>>(_tweetRepository.GetAll(x => x.CreatorId == userId).Select(x => _mapper.Map<TweetModel>(x)));
    }

    public Task<TweetModel> GetTweetById(Guid id)
    {
        ProcessException.ThrowIf(() =>_currentUserId != Guid.Empty &&  IsBanned(_currentUserId), "You are banned!");
        
        var tweet = _tweetRepository.GetById(id);
        return Task.FromResult(_mapper.Map<TweetModel>(tweet));
    }

    public Task DeleteTweet(Guid id)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var model = _tweetRepository.GetById(id);
        ProcessException.ThrowIf(() => _currentUserId != model.CreatorId && !IsAdmin(_currentUserId),
            "Only the creator of the tweet or admin can delete it.");
        
        _tweetRepository.Delete(_tweetRepository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<TweetModel> AddTweet(TweetModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var tweet = _mapper.Map<Tweet>(requestModel);
        tweet.CreatorId = _currentUserId;
        return Task.FromResult(_mapper.Map<TweetModel>(_tweetRepository.Save(tweet)));
    }

    public Task<TweetModel> UpdateTweet(Guid id, TweetModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var model = _tweetRepository.GetById(id);
        var tweet = _mapper.Map(requestModel, model);

        ProcessException.ThrowIf(() => tweet.CreatorId != _currentUserId,
            "Only the person who created it can change a tweet!");

        return Task.FromResult(_mapper.Map<TweetModel>(_tweetRepository.Save(tweet)));
    }

    public Task LikeTweet(Guid idTweet)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        var tweets = _userLikeTweetsRepository.GetAll(x => x.TweetId == idTweet && x.UserId == _currentUserId);

        // Если юзер уже лайкал твит, то убираем лайк
        if (tweets.Any())
            _userLikeTweetsRepository.Delete(tweets.First());
        else
            _userLikeTweetsRepository.Save(new UserLikeTweet {TweetId = idTweet, UserId = _currentUserId});
        return Task.CompletedTask;
    }
    
    private bool IsAdmin(Guid userId)
    {
        return _rolesUserRepository.GetAll(x => x.UserId == userId)
            .Any(x => x.Role.Permissions  == TwitterPermissions.Admin || x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
    }
    
    private bool IsBanned(Guid userId)
    {
        return _usersRepository.GetById(userId).IsBanned;
    }
}