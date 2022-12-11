using Microsoft.AspNetCore.Http;
using Twitter.FileService.Models;

namespace Twitter.FileService;
//Как делать update файлов: выбираем на форме какие файлы удалить из твита (делаем запрос Delete к их Id),
//добавляем новые файлы (делаем запрос на addfile)
public interface IFileService
{
    Task<IEnumerable<TwitterFileModel>> GetFiles();
    Task<TwitterFileModel> GetFileById(Guid id);
    Task DeleteFile(Guid id);
    Task<IEnumerable<TwitterFileModel>> AddFileToTweet(IEnumerable<IFormFile> files, Guid tweetId);
    Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<IFormFile> files, Guid commentId);
    Task<TwitterFileModel> AddAvatar(IFormFile file);
    Task<IEnumerable<TwitterFileModel>> AddFileToMessage(IEnumerable<IFormFile> files, Guid messageId);
    Task<IEnumerable<string>> GetTweetFiles(Guid tweetId);
    Task<IEnumerable<string>> GetCommentFiles(Guid commentId);
    Task<IEnumerable<string>> GetMessageFiles(Guid messageId);
    string GetAvatar(Guid userId);
    Task<IEnumerable<TwitterFileModel>> AddFileToTweet(IEnumerable<string> files, Guid tweetId);
    Task<IEnumerable<TwitterFileModel>> AddFileToComment(IEnumerable<string> files, Guid commentId);
    Task<TwitterFileModel> AddAvatar(string file);
    Task<IEnumerable<TwitterFileModel>> AddFileToMessage(IEnumerable<string> files, Guid messageId);
}