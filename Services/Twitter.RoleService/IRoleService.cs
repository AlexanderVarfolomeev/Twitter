using Twitter.RoleService.Models;

namespace Twitter.RoleService;

public interface IRoleService
{
    IEnumerable<TwitterRoleModel> GetRoles();
    TwitterRoleModel GetRoleById(Guid id);
    void DeleteRole(Guid id);
    TwitterRoleModel AddRole(TwitterRoleModelRequest requestModel);
    TwitterRoleModel UpdateRole(Guid id, TwitterRoleModelRequest requestModel);
    void GiveRole(Guid roleId, Guid userId);
    void RevokeRole(Guid roleId, Guid userId);
}