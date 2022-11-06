﻿using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Twitter.AccountService.Models;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace Twitter.AccountService;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TwitterUser> _accountsRepository;
    private readonly SignInManager<TwitterUser> _signInManager;
    private readonly IRepository<Subscribe> _subscribesRepository;
    private readonly IRepository<TwitterRoleTwitterUser> _rolesUserRepository;
    private readonly IRepository<TwitterRole> _rolesRepository;

    private readonly Guid _currentUserId;
    private readonly UserManager<TwitterUser> _userManager;

    public AccountService(SignInManager<TwitterUser> signInManager, UserManager<TwitterUser> userManager,
        IRepository<Subscribe> subscribesRepository,  IRepository<TwitterRoleTwitterUser> rolesUserRepository, IRepository<TwitterRole> rolesRepository,
        IRepository<TwitterUser> accountsRepository, IMapper mapper, IHttpContextAccessor accessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _subscribesRepository = subscribesRepository;
        _rolesUserRepository = rolesUserRepository;
        _rolesRepository = rolesRepository;
        _accountsRepository = accountsRepository;
        _mapper = mapper;
            
        //когда подключаемся с clientCredentials, ставим GUID empty
        var value = accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
        
    }


    public async Task<IEnumerable<TwitterAccountModel>> GetAccounts()
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty &&  IsBanned(_currentUserId), "You are banned!");
        
        var accounts = _accountsRepository.GetAll();
        var result = (await accounts.ToListAsync()).Select(x => _mapper.Map<TwitterAccountModel>(x));
        return result;
    }

    public Task<TwitterAccountModel> GetAccountById(Guid id)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), "You are banned!");
        
        var account = _accountsRepository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(account));
    }

    public Task DeleteAccount(Guid id)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        
        if (id != _currentUserId)
        {
            ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "Only the admin or the account owner can delete the account");
        }
        
        var account = _accountsRepository.GetById(id);
        _accountsRepository.Delete(account);
        return Task.CompletedTask;
    }

    public async Task<TwitterAccountModel> RegisterAccount(TwitterAccountModelRequest requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        ProcessException.ThrowIf(() => user is not null, "User with this email already exists!");

        user = _mapper.Map<TwitterUser>(requestModel);
        user.PhoneNumberConfirmed = false;
        user.EmailConfirmed = false;

        user.Init();
        GiveUserRole(user);

        var result = await _userManager.CreateAsync(user, requestModel.Password);
        ProcessException.ThrowIf(() => !result.Succeeded, result.ToString());
        return _mapper.Map<TwitterAccountModel>(user);
    }

    public Task<TwitterAccountModel> UpdateAccount(Guid id, TwitterAccountModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        ProcessException.ThrowIf(() => id != _currentUserId, "Only the account owner can change the account information.");
        
        var model = _accountsRepository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(_accountsRepository.Save(file)));
    }

    public Task Subscribe(Guid userId)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        
        // Пользователь не может подписаться сам на себя
        if (userId == _currentUserId) return Task.CompletedTask;

        var sub = _subscribesRepository.GetAll(x => x.SubscriberId == _currentUserId && x.UserId == userId);

        //Если пользователь уже подписан на данный аккаунт, то отписываемся
        if (sub.Any())
            _subscribesRepository.Delete(sub.First());
        else
            _subscribesRepository.Save(new Subscribe {SubscriberId = _currentUserId, UserId = userId});

        return Task.CompletedTask;
    }

    public Task BanUser(Guid userId)
    {
        ProcessException.ThrowIf(() => _currentUserId == Guid.Empty, "You can't do this with client credentials flow.");
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "Only the admin can ban.");
        ProcessException.ThrowIf(() => IsAdmin(userId), "You can't ban the admin.");
        
        var model = _accountsRepository.GetById(userId);
        model.IsBanned = !model.IsBanned;
        _accountsRepository.Save(model);
        return Task.CompletedTask;
    }

    private void GiveUserRole(TwitterUser user)
    {
        var userRoleId = _rolesRepository.GetAll(x => x.Permissions == TwitterPermissions.User).First().Id;
        _rolesUserRepository.Save(new TwitterRoleTwitterUser {RoleId = userRoleId, UserId = user.Id});
    }

    private bool IsAdmin(Guid userId)
    {
        return _rolesUserRepository.GetAll(x => x.UserId == userId)
            .Any(x => x.Role.Permissions  == TwitterPermissions.Admin || x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
    }

    private bool IsBanned(Guid userId)
    {
        return _accountsRepository.GetById(userId).IsBanned;
    }
}