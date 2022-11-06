﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.Entities.Users;
using Twitter.Repository;
using Twitter.RoleService.Models;

namespace Twitter.RoleService;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TwitterRole> _rolesRepository;
    private readonly IRepository<TwitterRoleTwitterUser> _rolesUserRepository;
    
    private readonly Guid _currentUserId;
    public RoleService(IRepository<TwitterRole> rolesRepository, IMapper mapper,
        IRepository<TwitterRoleTwitterUser> rolesUserRepository, IHttpContextAccessor accessor)
    {
        _rolesRepository = rolesRepository;
        _mapper = mapper;
        _rolesUserRepository = rolesUserRepository;
        
        
        _currentUserId =  Guid.Parse(accessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }


    public async Task<IEnumerable<TwitterRoleModel>> GetRoles()
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights.");
        
        var roles = _rolesRepository.GetAll();
        var result = (await roles.ToListAsync()).Select(x => _mapper.Map<TwitterRoleModel>(x));
        return result;
    }

    public Task<TwitterRoleModel> GetRoleById(Guid id)
    {
        ProcessException.ThrowIf(() =>  !IsAdmin(_currentUserId), "No access rights.");
        
        var role = _rolesRepository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(role));
    }

    public Task DeleteRole(Guid id)
    {
        ProcessException.ThrowIf(() =>  !IsAdmin(_currentUserId), "No access rights.");
        
        _rolesRepository.Delete(_rolesRepository.GetById(id));
        return Task.CompletedTask;
    }

    public Task<TwitterRoleModel> AddRole(TwitterRoleModelRequest requestModel)
    {
        ProcessException.ThrowIf(() =>  !IsAdmin(_currentUserId), "No access rights.");
        
        var model = _mapper.Map<TwitterRole>(requestModel);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(_rolesRepository.Save(model)));
    }

    public Task<TwitterRoleModel> UpdateRole(Guid id, TwitterRoleModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights.");
        
        var model = _rolesRepository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterRoleModel>(_rolesRepository.Save(file)));
    }

    public Task GiveRole(Guid roleId, Guid userId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "No access rights.");
        
        // Выдавать роли, может только админ с высшими правами
        ThrowIfNotFullAdmin();
        
        _rolesUserRepository.Save(new TwitterRoleTwitterUser {RoleId = roleId, UserId = userId});
        return Task.CompletedTask;
    }
    
    public Task RevokeRole(Guid roleId, Guid userId)
    {
        ProcessException.ThrowIf(() =>  !IsAdmin(_currentUserId), "No access rights.");
        
        // Нельзя отозвать роль юзера
        var revokeRole = _rolesRepository.GetById(roleId);
        ProcessException.ThrowIf(() => revokeRole.Permissions == TwitterPermissions.User, "Can't take away the role of the user");
        
        // Отнимать роли, может только админ с высшими правами
        ThrowIfNotFullAdmin();

        var userRole = _rolesUserRepository.GetAll(x => x.UserId == userId && x.RoleId == roleId).First();
        _rolesUserRepository.Delete(userRole);
        return Task.CompletedTask;
    }

    private void ThrowIfNotFullAdmin()
    {
        var isCurrentUserFullAdmin = _rolesUserRepository.GetAll(x => x.UserId == _currentUserId).Any(x => x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
        ProcessException.ThrowIf(() => !isCurrentUserFullAdmin, "Insufficient access rights.");
    }
    
    private bool IsAdmin(Guid userId)
    {
        return _rolesUserRepository.GetAll(x => x.UserId == userId)
            .Any(x => x.Role.Permissions  == TwitterPermissions.Admin || x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
    }
    
}