#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Controllers.HomePage;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("/")]
public class HomepageByIdController : ControllerBase
{
    [HttpGet]
    public RedirectResult Index()
    {
        return Redirect("/swagger");
    }
}