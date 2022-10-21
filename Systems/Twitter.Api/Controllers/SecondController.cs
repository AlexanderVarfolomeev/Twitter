using Microsoft.AspNetCore.Mvc;

namespace Twitter.Api.Controllers;
[ApiVersion("2.0")]
[ApiController]
public class SecondController : ControllerBase
{
    
    [HttpGet("get-v2")]
    public int Getint()
    {
        return 2;
    }
}