using Twitter.FileService.Models;

namespace Twitter.FileService;

public interface IFileService
{
    Task<IEnumerable<TwitterFileModel>> GetFiles();
    Task<TwitterFileModel> GetFileById(Guid id);
    Task DeleteFile(Guid id);
    Task<TwitterFileModel> AddFile(TwitterFileModelRequest requestModel);
    Task<TwitterFileModel> UpdateFile(Guid id, TwitterFileModelRequest requestModel);
}