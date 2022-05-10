using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet(Name = "GetHello")]
    public IEnumerable<string> Get()
    {
        return new List<string> { "Hello!" };
    }
}