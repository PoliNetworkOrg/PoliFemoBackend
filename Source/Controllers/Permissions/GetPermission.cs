#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Permission;
using PoliFemoBackend.Source.Utils;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

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
        List<PermissionGrantObject> perms = AuthUtil.GetPermissions(sub);
        if(perms == null){
            Response.StatusCode = 500;
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Internal server error" }
            });
        }
        
        Dictionary<string, List<string>> indexed = new Dictionary<string, List<string>>();
        for(int i = 0;i < perms.Count;i++){
            var name_grant = perms[i].name_grant;
            var id_object = perms[i].id_object;
            if(name_grant != null && id_object != null){
                if(!indexed.ContainsKey(name_grant))
                    indexed.Add(name_grant, (new List<string>()));
                indexed[name_grant].Add(id_object);
            }
        }  

        return Ok(
            new ObjectResult(indexed).Value
        );

    }
}