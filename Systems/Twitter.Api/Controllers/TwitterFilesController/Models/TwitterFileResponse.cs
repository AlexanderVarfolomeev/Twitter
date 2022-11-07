using AutoMapper;
using Shared;
using Twitter.FileService.Models;

namespace Twitter.Api.Controllers.TwitterFilesController.Models;

public class TwitterFileResponse
{
    public string Name { get; set; } = string.Empty;
    public TypeOfFile TypeOfFile { get; set; }
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}

public class TwitterFileResponseProfile : Profile
{
    public TwitterFileResponseProfile()
    {
        CreateMap<TwitterFileModel, TwitterFileResponse>();
    }
}