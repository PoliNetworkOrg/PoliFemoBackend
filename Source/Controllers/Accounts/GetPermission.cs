#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Permissions;
using PoliFemoBackend.Source.Utils;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("accounts/{id}/permissions")]
[Authorize]

public class GetPermissions : ControllerBase
{
    /// <summary>
    ///     Get the permissions of the user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="401">Authorization error</response>
    /// <response code="500">Can't connect to the server</response>
    
    [HttpGet]
    public ObjectResult GetPermission(string id)
    {
        var perms = AuthUtil.GetPermissions(id, false);

        if (perms.Count == 0)
        {
            Response.StatusCode = 404;
            return new NotFoundObjectResult(new JObject
            {
                { "error", "No permissions found" }
            });
        }

        var formattedPerms = Grant.GetFormattedPerms(perms);

        return Ok(
            new JObject
            {
                { "permissions", JToken.FromObject(formattedPerms) }
            }
        );
    }
}