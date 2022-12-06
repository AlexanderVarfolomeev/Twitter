using System.Security.Claims;
using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Enum;
using Shared.Exceptions;
using Shared.Security;
using Twitter.AccountService.Models;
using Twitter.Entities.Users;
using Twitter.Repository;
using Twitter.Settings.Interfaces;

namespace Twitter.AccountService;

public class AccountService : IAccountService
{
    private readonly IMapper _mapper;
    private readonly SignInManager<TwitterUser> _signInManager;
    private readonly ITwitterApiSettings _apiSettings;
    private readonly IRepository<TwitterUser> _accountsRepository;
    private readonly IRepository<Subscribe> _subscribesRepository;
    private readonly IRepository<TwitterRoleTwitterUser> _rolesUserRepository;
    private readonly UserManager<TwitterUser> _userManager;
    private readonly IRepository<TwitterRole> _rolesRepository;

    private readonly Guid _currentUserId;

    public AccountService(IRepository<Subscribe> subscribesRepository, IRepository<TwitterRoleTwitterUser> rolesUserRepository, UserManager<TwitterUser> userManager,  IRepository<TwitterRole> rolesRepository,
        IRepository<TwitterUser> accountsRepository, IMapper mapper, IHttpContextAccessor accessor,  SignInManager<TwitterUser> signInManager, ITwitterApiSettings apiSettings)
    {
        _subscribesRepository = subscribesRepository;
        _rolesUserRepository = rolesUserRepository;
        _userManager = userManager;
        _rolesRepository = rolesRepository;
        _accountsRepository = accountsRepository;
        _mapper = mapper;
        _signInManager = signInManager;
        _apiSettings = apiSettings;


        var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _currentUserId = value != null ? Guid.Parse(value) : Guid.Empty;
    }


    public IEnumerable<TwitterAccountModel> GetAccounts(int offset = 0, int limit = 10)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), "You are banned!");

        var accounts = _accountsRepository.GetAll()
            .Skip(Math.Max(offset, 0))
            .Take(Math.Max(0, Math.Min(limit, 1000)));
        
        return _mapper.Map<IEnumerable<TwitterAccountModel>>(accounts);
    }

    public TwitterAccountModel GetAccountById(Guid id)
    {
        ProcessException.ThrowIf(() => _currentUserId != Guid.Empty && IsBanned(_currentUserId), "You are banned!");

        var account = _accountsRepository.GetById(id);
        return _mapper.Map<TwitterAccountModel>(account);
    }

    public void DeleteAccount(Guid id)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");

        if (id != _currentUserId)
            ProcessException.ThrowIf(() => !IsAdmin(_currentUserId),
                "Only the admin or the account owner can delete the account");

        var account = _accountsRepository.GetById(id);
        _accountsRepository.Delete(account);
    }

    public TwitterAccountModel UpdateAccount(Guid id, TwitterAccountModelRequest requestModel)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");
        ProcessException.ThrowIf(() => id != _currentUserId,
            "Only the account owner can change the account information.");

        var model = _accountsRepository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return _mapper.Map<TwitterAccountModel>(_accountsRepository.Save(file));
    }

    public void Subscribe(Guid userId)
    {
        ProcessException.ThrowIf(() => IsBanned(_currentUserId), "You are banned!");

        // Пользователь не может подписаться сам на себя
        if (userId == _currentUserId) return;

        var sub = _subscribesRepository.GetAll(x => x.SubscriberId == _currentUserId && x.UserId == userId);

        //Если пользователь уже подписан на данный аккаунт, то отписываемся
        if (sub.Any())
            _subscribesRepository.Delete(sub.First());
        else
            _subscribesRepository.Save(new Subscribe {SubscriberId = _currentUserId, UserId = userId});
    }
    
     public async Task<TwitterAccountModel> RegisterUser(TwitterAccountModelRequest requestModel)
    {
        var user = await _userManager.FindByEmailAsync(requestModel.Email);
        ProcessException.ThrowIf(() => user is not null, "User with this email already exists!");

        user = _mapper.Map<TwitterUser>(requestModel);
        user.PhoneNumberConfirmed = false;
        user.EmailConfirmed = false;

        user.Init();
        var result =  await _userManager.CreateAsync(user, requestModel.Password);
        ProcessException.ThrowIf(() => !result.Succeeded, result.ToString());
        GiveUserRole(user);


        return _mapper.Map<TwitterAccountModel>(user);
    }

    public async Task<TokenResponse> LoginUser(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        ProcessException.ThrowIf(() => user is null, "User not found");

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        ProcessException.ThrowIf(() => !result.Succeeded, "Incorrect email or password");

        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(_apiSettings.Duende.Url);
        ProcessException.ThrowIf(() => disco.IsError, disco.Error);

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = disco.TokenEndpoint,
            ClientId = model.ClientId,
            ClientSecret = model.ClientSecret,
            Password = model.Password,
            UserName = model.Username,
            Scope = AppScopes.TwitterRead + " " +AppScopes.TwitterWrite
        });
        
        ProcessException.ThrowIf(() => tokenResponse.IsError, tokenResponse.Error);
        return tokenResponse;
    }

    private void GiveUserRole(TwitterUser user)
    {
        var userRoleId = _rolesRepository.GetAll(x => x.Permissions == TwitterPermissions.User).First().Id;
        _rolesUserRepository.Save(new TwitterRoleTwitterUser {RoleId = userRoleId, UserId = user.Id});
    }

    public void BanUser(Guid userId)
    {
        ProcessException.ThrowIf(() => !IsAdmin(_currentUserId), "Only the admin can ban.");
        ProcessException.ThrowIf(() => IsAdmin(userId), "You can't ban the admin.");

        var model = _accountsRepository.GetById(userId);
        model.IsBanned = !model.IsBanned;
        _accountsRepository.Save(model);
    }


    private bool IsAdmin(Guid userId)
    {
        return _rolesUserRepository.GetAll(x => x.UserId == userId)
            .Any(x => x.Role.Permissions == TwitterPermissions.Admin ||
                      x.Role.Permissions == TwitterPermissions.FullAccessAdmin);
    }

    private bool IsBanned(Guid userId)
    {
        return _accountsRepository.GetById(userId).IsBanned;
    }
}