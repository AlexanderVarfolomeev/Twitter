using Twitter.ReportServices.Models;

namespace Twitter.ReportServices;

public interface IReportService
{
    Task<IEnumerable<ReportModel>> GetReportsToTweets();
    Task<IEnumerable<ReportModel>> GetReportsToComments();
    Task<IEnumerable<ReportModel>> GetReportsByTweet(Guid tweetId);
    Task<IEnumerable<ReportModel>> GetReportsByComment(Guid commentId);
    Task CloseReportToComment(Guid reportId);
    Task CloseReportToTweet(Guid reportId);

    Task<ReportModel> AddTweetReport(ReportModelRequest modelRequest, Guid tweetId);
    Task<ReportModel> AddCommentReport(ReportModelRequest modelRequest, Guid commentId);
}