using AutoMapper;
using Twitter.FileService.Models;

namespace Twitter.Api.Controllers.TwitterFilesController.Models;

public class TwitterFileRequest
{
    public string Name { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
}

public class TwitterFileRequestProfile : Profile
{
    public TwitterFileRequestProfile()
    {
        CreateMap<TwitterFileRequest, TwitterFileModelRequest>();
    }
}