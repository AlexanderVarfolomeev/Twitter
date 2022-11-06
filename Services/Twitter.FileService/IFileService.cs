using Microsoft.AspNetCore.Http;
using Twitter.FileService.Models;

namespace Twitter.FileService;

public interface IFileService
{
    Task<IEnumerable<TwitterFileModel>> GetFiles();
    Task<TwitterFileModel> GetFileById(Guid id);
    Task DeleteFile(Guid id);
    Task<IEnumerable<TwitterFileModel>> AddFileToTweet(IEnumerable<IFormFile> files, Guid tweetId);
    Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<IFormFile> files, Guid commentId);
    Task<IEnumerable<string>> GetTweetFiles(Guid tweetId);
    Task<IEnumerable<string>> GetCommentFiles(Guid commentId);
    Task<string> GetAvatar(Guid userId);
    Task<TwitterFileModel> UpdateFile(Guid id, TwitterFileModelRequest requestModel);
}