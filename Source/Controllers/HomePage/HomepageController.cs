#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;

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
        return Redirect(GlobalVariables.BasePath + "/swagger" ?? "/swagger");
    }
}