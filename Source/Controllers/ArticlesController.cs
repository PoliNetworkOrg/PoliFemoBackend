using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(JObject payload)
    {
        
        return Ok(payload);
    }
}