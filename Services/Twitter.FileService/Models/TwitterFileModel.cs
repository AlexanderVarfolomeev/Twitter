﻿using AutoMapper;
using Twitter.Entities.Base;

namespace Twitter.FileService.Models;

public class TwitterFileModel : BaseEntity
{
    public string Name { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
}

public class TwitterFileModelProfile : Profile
{
    public TwitterFileModelProfile()
    {
        CreateMap<TwitterFile, TwitterFileModel>();
    }
}