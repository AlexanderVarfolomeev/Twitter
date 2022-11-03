using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Exceptions;
using Twitter.AccountService.Models;
using Twitter.Entities.Users;
using Twitter.Repository;

namespace Twitter.AccountService;

public class AccountService : IAccountService
{
    private readonly SignInManager<TwitterUser> _signInManager;
    private readonly UserManager<TwitterUser> _userManager;
    private readonly IRepository<Subscribe> _subscribesRepository;
    private readonly IRepository<TwitterUser> _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _accessor;
    
    private readonly Guid _userId;

    public AccountService(SignInManager<TwitterUser> signInManager, UserManager<TwitterUser> userManager, IRepository<Subscribe> subscribesRepository,
        IRepository<TwitterUser> repository, IMapper mapper, IHttpContextAccessor accessor)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _subscribesRepository = subscribesRepository;
        _repository = repository;
        _mapper = mapper;
        _accessor = accessor;
        
        _userId = Guid.Parse(accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
    }


    public async Task<IEnumerable<TwitterAccountModel>> GetAccounts()
    {
        var accounts = _repository.GetAll();
        var result = (await accounts.ToListAsync()).Select(x => _mapper.Map<TwitterAccountModel>(x));
        return result;
    }

    public Task<TwitterAccountModel> GetAccountById(Guid id)
    {
        var account = _repository.GetById(id);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(account));
    }

    public Task DeleteAccount(Guid id)
    {
        var account = _repository.GetById(id);
        _repository.Delete(account);
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
        
        
        
        var result = await _userManager.CreateAsync(user, requestModel.Password);
        ProcessException.ThrowIf(() => !result.Succeeded, result.ToString());
        return _mapper.Map<TwitterAccountModel>(user);
    }

    public Task<TwitterAccountModel> UpdateAccount(Guid id, TwitterAccountModelRequest requestModel)
    {
        var model = _repository.GetById(id);
        var file = _mapper.Map(requestModel, model);
        return Task.FromResult(_mapper.Map<TwitterAccountModel>(_repository.Save(file)));
    }

    public Task Subscribe(Guid userId)
    {
        // Пользователь не может подписаться сам на себя
        if (userId == _userId)
        {
            return Task.CompletedTask;
        }
        
        var sub = _subscribesRepository.GetAll((x) => x.SubscriberId == _userId && x.UserId == userId);
        
        //Если пользователь уже подписан на данный аккаунт, то отписываемся
        if (sub.Any())
        {
            _subscribesRepository.Delete(sub.First());
        }
        else
        {
            _subscribesRepository.Save(new Subscribe() {SubscriberId = _userId, UserId = userId});
        }
        
        return Task.CompletedTask;
    }
}

