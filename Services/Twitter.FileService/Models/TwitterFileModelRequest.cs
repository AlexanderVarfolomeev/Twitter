using AutoMapper;
using Twitter.Entities.Base;

namespace Twitter.FileService.Models;

public class TwitterFileModelRequest
{
    public string Name { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
}

public class TwitterFileModelRequestProfile  : Profile
{
    public TwitterFileModelRequestProfile()
    {
        CreateMap<TwitterFileModelRequest, TwitterFile>();
    }
}