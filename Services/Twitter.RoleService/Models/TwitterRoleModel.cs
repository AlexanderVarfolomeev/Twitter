using AutoMapper;
using Shared.Enum;
using Twitter.Entities.Users;

namespace Twitter.RoleService.Models;

public class TwitterRoleModel
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public TwitterPermissions Permissions { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
}

public class TwitterRoleModelProfile : Profile
{
    public TwitterRoleModelProfile()
    {
        CreateMap<TwitterRole, TwitterRoleModel>();
    }
}