#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Profile;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Account")]
[Route("v{version:apiVersion}/accounts/{id}/permissions")]
[Route("accounts/{id}/permissions")]
public class GetPermissions : ControllerBase
{
    /// <summary>
    ///     Returns the permissions of the user
    /// </summary>
    /// <remarks>
    /// returns the permissions in the following format: 
    /// {
    /// "permission1" : [object_id1, object_id2, ...],
    /// "permission2" : [object_id1, object_id2, ...]
    /// ......
    /// }
    /// </remarks>
    /// <param name="id">id of the user</param>
    /// <response code="200">Permissions returned successfully</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpPost]
    public ObjectResult GetPermission(string id)
    {
        var perms = AuthUtil.GetPermissions(id);
        if(perms == null){
            Response.StatusCode = 500;
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Internal server error" }
            });
        }
        
        var indexed = new Dictionary<string, List<string>>();
        foreach (var t in perms)
        {
            var name_grant = t.name_grant;
            var id_object = t.id_object;
            if (string.IsNullOrEmpty(name_grant) || string.IsNullOrEmpty(id_object))
                continue;
            
            if(!indexed.ContainsKey(name_grant))
                indexed.Add(name_grant, new List<string>());
            
            indexed[name_grant].Add(id_object);
        }

        if (indexed.Keys.Count == 0)
        {
            Response.StatusCode = 404;
            return new NotFoundObjectResult(new JObject
            {
                { "error", "No permissions found" }
            });
        }

        return Ok(
            new ObjectResult(indexed).Value
        );

    }
}