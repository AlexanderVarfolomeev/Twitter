using AutoMapper;
using Shared;
using Twitter.Entities.Base;

namespace Twitter.FileService.Models;

public class TwitterFileModel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public TypeOfFile Type { get; set; }
}

public class TwitterFileModelProfile : Profile
{
    public TwitterFileModelProfile()
    {
        CreateMap<TwitterFile, TwitterFileModel>();
    }
}