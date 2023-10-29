#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Permissions;
using PoliFemoBackend.Source.Utils.Auth;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiExplorerSettings(GroupName = "Permissions")]
[Route("accounts/{id}/permissions")]
[Authorize]
public class GetPermissionsController : ControllerBase
{
    /// <summary>
    ///     Get the permissions of the user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="401">Authorization error</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult GetPermissions(string id)
    {
        var perms = AccountAuthUtil.GetPermissions(id, false);

        if (perms.Count == 0)
        {
            Response.StatusCode = 404;
            return new NotFoundObjectResult(new JObject { { "error", "No permissions found" } });
        }

        var formattedPerms = Grant.GetFormattedPerms(perms);

        return Ok(new JObject { { "permissions", JToken.FromObject(formattedPerms) } });
    }
}
