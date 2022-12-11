using AutoMapper;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.AccountService;
using Twitter.AccountService.Models;
using Twitter.Api.Controllers.AccountController.Models;
using TwitterAccountModelRequest = Twitter.AccountService.Models.TwitterAccountModelRequest;

namespace Twitter.Api.Controllers.AccountController;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TwitterAccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public TwitterAccountController(IMapper mapper, IAccountService accountService)
    {
        _mapper = mapper;
        _accountService = accountService;
    }

    [HttpPost("")]
    public async Task<TwitterAccountResponse> RegisterUser([FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(await _accountService.RegisterUser(model));
    }

    [HttpPost("login")]
    public async Task<TokenResponse> LoginUser([FromBody] LoginModelRequest model)
    {
        return await _accountService.LoginUser(_mapper.Map<LoginModel>(model));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("")]
    public IEnumerable<TwitterAccountResponse> GetAccounts([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _accountService.GetAccounts(offset, limit)
            .Select(x => _mapper.Map<TwitterAccountResponse>(x));
    }
    
    
    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-subscribers-{userId}")]
    public IEnumerable<TwitterAccountResponse> GetSubsribers([FromRoute] Guid userId,[FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _accountService.GetSubscribers(userId,offset, limit)
            .Select(x => _mapper.Map<TwitterAccountResponse>(x));
    } 
    
    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("get-subscriptions-{userId}")]
    public IEnumerable<TwitterAccountResponse> GetSubsriptions([FromRoute] Guid userId,[FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _accountService.GetSubscriptions(userId,offset, limit)
            .Select(x => _mapper.Map<TwitterAccountResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("{id}")]
    public TwitterAccountResponse GetAccountById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterAccountResponse>(_accountService.GetAccountById(id));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount([FromRoute] Guid id)
    {
        _accountService.DeleteAccount(id);
        return Ok();
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("{id}")]
    public TwitterAccountResponse UpdateAccount([FromRoute] Guid id,
        [FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(_accountService.UpdateAccount(id, model));
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPost("Subscribe")]
    public IActionResult SubscribeTo([FromQuery] Guid id)
    {
        _accountService.Subscribe(id);
        return Ok();
    }

    [Authorize(AppScopes.TwitterWrite)]
    [HttpPut("ban-user")]
    public IActionResult BanUser([FromQuery] Guid id)
    {
        _accountService.BanUser(id);
        return Ok();
    }
}