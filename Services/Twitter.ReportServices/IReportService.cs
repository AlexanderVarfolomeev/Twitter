using Twitter.ReportServices.Models;

namespace Twitter.ReportServices;

public interface IReportService
{
    Task<IEnumerable<ReportModel>> GetReportsToTweets( int offset = 0, int limit = 10);
    Task<IEnumerable<ReportModel>> GetReportsToComments( int offset = 0, int limit = 10);
    Task<IEnumerable<ReportModel>> GetReportsByTweet(Guid tweetId,  int offset = 0, int limit = 10);
    Task<IEnumerable<ReportModel>> GetReportsByComment(Guid commentId,  int offset = 0, int limit = 10);
    Task CloseReportToComment(Guid reportId);
    Task CloseReportToTweet(Guid reportId);

    Task<ReportModel> AddTweetReport(ReportModelRequest modelRequest, Guid tweetId);
    Task<ReportModel> AddCommentReport(ReportModelRequest modelRequest, Guid commentId);
}