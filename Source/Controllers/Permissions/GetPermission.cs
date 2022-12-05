#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Permission;
using PoliFemoBackend.Source.Utils;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Permissions;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Permissions")]
[Route("v{version:apiVersion}/permissions")]
[Route("/permissions")]
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
    /// <response code="200">Permissions returned successfully</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [Authorize]
    public ObjectResult GetPermission()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var perms = AuthUtil.GetPermissions(sub);
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
            if (name_grant == null || id_object == null)
                continue;
            
            if(!indexed.ContainsKey(name_grant))
                indexed.Add(name_grant, (new List<string>()));
            indexed[name_grant].Add(id_object);
        }  

        return Ok(
            new ObjectResult(indexed).Value
        );

    }
}