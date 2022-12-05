using Twitter.ReportServices.Models;

namespace Twitter.ReportServices;

public interface IReportService
{
    IEnumerable<ReportModel> GetReportsToTweets( int offset = 0, int limit = 10);
    IEnumerable<ReportModel> GetReportsToComments( int offset = 0, int limit = 10);
    IEnumerable<ReportModel> GetReportsByTweet(Guid tweetId,  int offset = 0, int limit = 10);
    IEnumerable<ReportModel> GetReportsByComment(Guid commentId,  int offset = 0, int limit = 10);
    void CloseReportToComment(Guid reportId);
    void CloseReportToTweet(Guid reportId);

    ReportModel AddTweetReport(ReportModelRequest modelRequest, Guid tweetId);
    ReportModel AddCommentReport(ReportModelRequest modelRequest, Guid commentId);
}