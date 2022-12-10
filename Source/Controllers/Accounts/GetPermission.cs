#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Permission;
using PoliFemoBackend.Source.Utils;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("v{version:apiVersion}/accounts/{id}/permissions")]
[Route("accounts/{id}/permissions")]
[Authorize]
public class GetPermissions : ControllerBase
{
    /// <summary>
    ///     Returns the permissions of the user
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <response code="200">Permissions returned successfully</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult GetPermission(string id)
    {
        var perms = AuthUtil.GetPermissions(id, false);
        if (perms == null)
        {
            Response.StatusCode = 500;
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Internal server error" }
            });
        }

        if (perms.Count == 0)
        {
            Response.StatusCode = 404;
            return new NotFoundObjectResult(new JObject
            {
                { "error", "No permissions found" }
            });
        }

        var formattedPerms = PermissionGrantObject.GetFormattedPerms(perms);

        return Ok(
            new JObject
            {
                { "permissions", JToken.FromObject(formattedPerms) }
            }
        );
    }
}