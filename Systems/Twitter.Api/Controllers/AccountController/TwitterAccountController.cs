using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Security;
using Twitter.AccountService;
using Twitter.AccountService.Models;
using Twitter.Api.Controllers.AccountController.Models;

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

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("")]
    public IEnumerable<TwitterAccountResponse> GetAccounts([FromQuery] int offset = 0, [FromQuery] int limit = 10)
    {
        return _accountService.GetAccounts(offset, limit)
            .Select(x => _mapper.Map<TwitterAccountResponse>(x));
    }

    [Authorize(AppScopes.TwitterRead)]
    [HttpGet("{id}")]
    public TwitterAccountResponse GetAccountById([FromRoute] Guid id)
    {
        return _mapper.Map<TwitterAccountResponse>(_accountService.GetAccountById(id));
    }

    [HttpPost("")]
    public TwitterAccountResponse RegisterAccount([FromBody] TwitterAccountRequest account)
    {
        var model = _mapper.Map<TwitterAccountModelRequest>(account);
        return _mapper.Map<TwitterAccountResponse>(_accountService.RegisterAccount(model));
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