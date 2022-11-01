﻿using AutoMapper;
using Twitter.Entities.Users;

namespace Twitter.AccountService.Models;

public class TwitterAccountModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    
    public string Name { get; set; } = String.Empty;
    public string Surname { get; set; } = String.Empty;
    public string? Patronymic { get; set; }
    public DateTime Birthday { get; set; }
    public string PageDescription { get; set; } = String.Empty;
    public bool IsBanned { get; set; } = false;
    
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    
    public string PasswordHash { get; set; } = String.Empty;
    
    public Guid? AvatarId { get; set; }
}

public class TwitterAccountModelProfile : Profile
{
    public TwitterAccountModelProfile()
    {
        CreateMap<TwitterUser, TwitterAccountModel>();
    }
}