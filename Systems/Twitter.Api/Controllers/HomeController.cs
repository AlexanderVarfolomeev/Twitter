using Microsoft.AspNetCore.Mvc;

namespace Twitter.Api.Controllers;
[ApiVersion("1.0")]
[ApiController]
public class HomeController : ControllerBase
{

    [HttpGet("{id}")]
    public (string, int) GetInfo(int id)
    {
        return ("return", id);
    }
}