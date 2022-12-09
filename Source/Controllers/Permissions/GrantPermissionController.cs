using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Permissions;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Permissions")]
[Route("v{version:apiVersion}/permissions/grant")]
[Route("/permissions/grant")]
public class GrantPermissionController : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPost]
    [Authorize]
    public ObjectResult GrantPermission(string idGrant, string idUser, long idObject)
    {
        // id_grant, id_user, id_object
        const string q = "INSERT INTO permission (id_grant, id_user, id_object) VALUES (@id_grant, @id_user, @id_object)";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>()
        {
            {"@id_grant",idGrant},
            {"@id_user",idUser},
            {"@id_object",idObject},
        });
        
        if (count > 0)
            return Ok("");
        
        return new BadRequestObjectResult(new JObject
        {
            { "error", "Grant failed" }
        });
    }
}