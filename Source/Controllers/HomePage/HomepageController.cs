#region

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Objects.Permission;
using PoliFemoBackend.Source.Utils;

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