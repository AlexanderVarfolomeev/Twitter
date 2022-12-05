using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twitter.Api.Controllers.TwitterRolesController.Models;
using Twitter.RoleService;
using Twitter.RoleService.Models;

namespace Twitter.Api.Controllers.TwitterRolesController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class TwitterRolesController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public TwitterRolesController(IRoleService _roleService, IMapper mapper)
    {
        this._roleService = _roleService;
        _mapper = mapper;
    }

    [HttpGet("")]
    public IEnumerable<TwitterRoleResponse> GetRoles()
    {
        return _roleService.GetRoles().Select(x => _mapper.Map<TwitterRoleResponse>(x));
    }

    [HttpGet("{id}")]
    public TwitterRoleResponse GetRoleById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterRoleResponse>( _roleService.GetRoleById(id));
    }

    [HttpPost("")]
    public TwitterRoleResponse AddRole([FromBody] TwitterRoleRequest role)
    {
        var model = _mapper.Map<TwitterRoleModelRequest>(role);
        return _mapper.Map<TwitterRoleResponse>( _roleService.AddRole(model));
    }

    [HttpPost("give-role")]
    public IActionResult GiveRole([FromQuery] Guid roleId, [FromQuery] Guid userId)
    {
        _roleService.GiveRole(roleId, userId);
        return Ok();
    }

    [HttpPost("revoke-role")]
    public IActionResult RevokeRole([FromQuery] Guid roleId, [FromQuery] Guid userId)
    {
        _roleService.RevokeRole(roleId, userId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRole([FromRoute] Guid id)
    {
        _roleService.DeleteRole(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public TwitterRoleResponse UpdateRole([FromRoute] Guid id, [FromBody] TwitterRoleRequest role)
    {
        var model = _mapper.Map<TwitterRoleModelRequest>(role);
        return _mapper.Map<TwitterRoleResponse>( _roleService.UpdateRole(id, model));
    }
}