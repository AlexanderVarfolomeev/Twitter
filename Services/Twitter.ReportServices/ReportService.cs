using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.Entities.Base;
using Twitter.Entities.Comments;
using Twitter.Entities.Tweets;
using Twitter.Entities.Users;
using Twitter.ReportServices.Models;
using Twitter.Repository;

namespace Twitter.ReportServices;

public class ReportService : IReportService
{
    private readonly IRepository<ReportToComment> _reportToCommentRepository;
    private readonly IRepository<ReportToTweet> _reportToTweetRepository;
    private readonly IRepository<ReasonReport> _reasonReportRepository;
    private readonly IRepository<TwitterUser> _accountRepository;
    private readonly IMapper _mapper;

    private readonly Guid _currentUserId;

    public ReportService(IRepository<ReportToComment> reportToCommentRepository,
        IRepository<ReportToTweet> reportToTweetRepository,
        IRepository<ReasonReport> reasonReportRepository, IHttpContextAccessor accessor,
        IRepository<TwitterUser> accountRepository, IMapper mapper)
    {
        _reportToCommentRepository = reportToCommentRepository;
        _reportToTweetRepository = reportToTweetRepository;
        _reasonReportRepository = reasonReportRepository;
        _accountRepository = accountRepository;
        _mapper = mapper;

        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }

    public IEnumerable<ReportModel> GetReportsToTweets( int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && !IsAdmin(_currentUserId), ErrorMessage.AccessRightsError);

        // Если жалоба была закрыта, то админу не нужно ее больше рассматривать
        var reportsToTweets = _reportToTweetRepository.GetAll(x => x.CloseDate == DateTime.MinValue)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        return _mapper.Map<IEnumerable<ReportModel>>(reportsToTweets);
    }

    public IEnumerable<ReportModel> GetReportsToComments( int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && !IsAdmin(_currentUserId), ErrorMessage.AccessRightsError);

        var reportsToComments = _reportToCommentRepository.GetAll(x => x.CloseDate == DateTime.MinValue)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        return _mapper.Map<IEnumerable<ReportModel>>(reportsToComments);
    }

    public IEnumerable<ReportModel> GetReportsByTweet(Guid tweetId,  int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && !IsAdmin(_currentUserId), ErrorMessage.AccessRightsError);

        var reportsToTweets = _reportToTweetRepository.GetAll(x => x.TweetId == tweetId)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        return _mapper.Map<IEnumerable<ReportModel>>(reportsToTweets);
    }

    public IEnumerable<ReportModel> GetReportsByComment(Guid commentId,  int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && !IsAdmin(_currentUserId), ErrorMessage.AccessRightsError);

        var reportsToComments = _reportToCommentRepository.GetAll(x => x.CommentId == commentId)
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));

        return _mapper.Map<IEnumerable<ReportModel>>(reportsToComments);
    }

    public void CloseReportToTweet(Guid reportId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), ErrorMessage.AccessRightsError);

        var report = _reportToTweetRepository.GetById(reportId);
        report.CloseDate = DateTime.Now;
        _reportToTweetRepository.Save(report);

    }

    // При закрытии жалобы, админ решает что делать с юзером (забанить, удалить коммент, твит) - на стороне клиента?
    public void CloseReportToComment(Guid reportId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights!");

        var report = _reportToCommentRepository.GetById(reportId);
        report.CloseDate = DateTime.Now;
        _reportToCommentRepository.Save(report);

    }

    public ReportModel AddTweetReport(ReportModelRequest modelRequest, Guid tweetId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _mapper.Map<ReportToTweet>(modelRequest);
        model.CreatorId = _currentUserId;
        model.TweetId = tweetId;

        return _mapper.Map<ReportModel>(_reportToTweetRepository.Save(model));
    }

    public ReportModel AddCommentReport(ReportModelRequest modelRequest, Guid commentId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), ErrorMessage.YouBannedError);

        var model = _mapper.Map<ReportToComment>(modelRequest);
        model.CreatorId = _currentUserId;
        model.CommentId = commentId;

        return _mapper.Map<ReportModel>(_reportToCommentRepository.Save(model));
    }

    private bool IsAdmin(Guid userId)
    {
        return _accountRepository.GetById(userId).TwitterRoles.Any(x =>
            x.Role.Permissions is TwitterPermissions.Admin or TwitterPermissions.FullAccessAdmin);
    }

    private bool IsBanned(Guid userId)
    {
        return _accountRepository.GetById(userId).IsBanned;
    }
}