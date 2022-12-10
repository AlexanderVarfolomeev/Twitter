using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.CommentsService.Models;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace Twitter.CommentsService;

public class CommentService : ICommentsService
{
    private readonly IRepository<Comment> _commentsRepository;
    private readonly IRepository<TwitterUser> _accountsRepository;
    private readonly IRepository<Tweet> _tweetRepository;
    private readonly IMapper _mapper;

    private readonly Guid _currentUserId;

    public CommentService(IHttpContextAccessor accessor, IRepository<Comment> commentsRepository,
        IRepository<TwitterUser> accountsRepository, IRepository<Tweet> tweetRepository, IMapper mapper)
    {
        _commentsRepository = commentsRepository;
        _accountsRepository = accountsRepository;
        _tweetRepository = tweetRepository;
        _mapper = mapper;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }

    public CommentModel AddComment(CommentModelRequest modelRequest, Guid tweetId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _mapper.Map<Comment>(modelRequest);
        model.CreatorId = _currentUserId;
        model.TweetId = tweetId;

        return _mapper.Map<CommentModel>(_commentsRepository.Save(model));
    }

    public IEnumerable<CommentModel> GetCommentsByTweet(Guid tweetId, int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var comments = _tweetRepository.GetById(tweetId).Comments
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        
        return _mapper.Map<IEnumerable<CommentModel>>(comments);
    }

    public void DeleteComment(Guid id)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var comment = _commentsRepository.GetById(id);
        if (comment.Creator.Id != _currentUserId)
        {
            var isAdmin = _accountsRepository.GetById(_currentUserId).TwitterRoles
                .Any(x => x.Role.Permissions is TwitterPermissions.Admin or TwitterPermissions.FullAccessAdmin);

            ProcessException.ThrowIf(() => !isAdmin,
                ErrorMessage.OnlyAdminOrAccountOwnerCanDoIdError);
        }

        _commentsRepository.Delete(_commentsRepository.GetById(id));
    }

    public CommentModel UpdateComment(Guid id, CommentModelRequest modelRequest)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _commentsRepository.GetById(id);
        var comment = _mapper.Map(modelRequest, model);

        ProcessException.ThrowIf(() => comment.CreatorId != _currentUserId,
            ErrorMessage.OnlyAccountOwnerCanDoIdError);

        return _mapper.Map<CommentModel>(_commentsRepository.Save(comment));
    }

    public IEnumerable<CommentModel> GetCommentsByUser(Guid userId)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        _accountsRepository.GetById(userId);
        var commentsByUser = _commentsRepository.GetAll(x => x.Creator.Id == userId);
        return _mapper.Map<IEnumerable<CommentModel>>(commentsByUser);
    }


    private bool IsBanned(Guid userId)
    {
        return _accountsRepository.GetById(userId).IsBanned;
    }
}