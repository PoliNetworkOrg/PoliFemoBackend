#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[Route("/")]
public class HomepageByIdController : ControllerBase
{
    [HttpGet]
    public string Index()
    {
        return "I'm up";
    }
}