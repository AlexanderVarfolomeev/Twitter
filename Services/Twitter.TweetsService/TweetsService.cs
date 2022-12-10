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
        IRepository<TwitterUser> usersRepository, IRepository<UserLikeTweet> userLikeTweetsRepository,
        IRepository<TwitterRoleTwitterUser> rolesUserRepository)
    {
        _tweetRepository = tweetRepository;
        _mapper = mapper;
        _usersRepository = usersRepository;
        _userLikeTweetsRepository = userLikeTweetsRepository;
        _rolesUserRepository = rolesUserRepository;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }

    public IEnumerable<TweetModel> GetTweets(int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var tweets = _tweetRepository.GetAll()
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        return _mapper.Map<IEnumerable<TweetModel>>(tweets);
    }

    public IEnumerable<TweetModel> GetTweetsBySubscribes(int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);
        var subscribes = _usersRepository.GetById(_currentUserId).Subscribes.Select(x => x.UserId);

        var tweets = _tweetRepository.GetAll(x => subscribes.Contains(x.CreatorId))
            .OrderByDescending(x => x.CreationTime)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        
        return _mapper.Map<IEnumerable<TweetModel>>(tweets).OrderByDescending(x => x.CreationTime);
    }

    public IEnumerable<TweetModel> GetTweetsForLastDays(int days, int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);
        var startDate = DateTime.Now - new TimeSpan(days, 0, 0, 0);
        var tweets = _tweetRepository.GetAll(x => x.CreationTime >= startDate)
            .OrderByDescending(x => x.CreationTime)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        
        return _mapper.Map<IEnumerable<TweetModel>>(tweets).OrderByDescending(x => x.CreationTime);
    }

    public IEnumerable<TweetModel> GetTweetsByUserId(Guid userId, int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        return _tweetRepository.GetAll(x => x.CreatorId == userId)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)))
            .Select(x => _mapper.Map<TweetModel>(x));
    }

    public TweetModel GetTweetById(Guid id)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var tweet = _tweetRepository.GetById(id);
        return _mapper.Map<TweetModel>(tweet);
    }

    public void DeleteTweet(Guid id)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _tweetRepository.GetById(id);
        ProcessException.ThrowIf(() => _currentUserId != model.CreatorId && !IsAdmin(_currentUserId),
            "Only the creator of the tweet or admin can delete it.");

        _tweetRepository.Delete(_tweetRepository.GetById(id));
    }

    public TweetModel AddTweet(TweetModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var tweet = _mapper.Map<Tweet>(requestModel);
        tweet.CreatorId = _currentUserId;
        return _mapper.Map<TweetModel>(_tweetRepository.Save(tweet));
    }

    public TweetModel UpdateTweet(Guid id, TweetModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _tweetRepository.GetById(id);
        var tweet = _mapper.Map(requestModel, model);

        ProcessException.ThrowIf(() => tweet.CreatorId != _currentUserId,
            ErrorMessage.OnlyAccountOwnerCanDoIdError);

        return _mapper.Map<TweetModel>(_tweetRepository.Save(tweet));
    }

    public void LikeTweet(Guid idTweet)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var tweets = _userLikeTweetsRepository.GetAll(x => x.TweetId == idTweet && x.UserId == _currentUserId);

        // Если юзер уже лайкал твит, то убираем лайк
        if (tweets.Any())
            _userLikeTweetsRepository.Delete(tweets.First());
        else
            _userLikeTweetsRepository.Save(new UserLikeTweet {TweetId = idTweet, UserId = _currentUserId});
    }

    public int GetCountLikesOfTweet(Guid id)
    {
        return _tweetRepository.GetById(id).Likes.Count;
    }

    private bool IsAdmin(Guid userId)
    {
        return _rolesUserRepository.GetAll(x => x.UserId == userId)
            .Any(x => x.Role.Permissions == TwitterPermissions.Admin ||
                      x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
    }

    private bool IsBanned(Guid userId)
    {
        return _usersRepository.GetById(userId).IsBanned;
    }
}