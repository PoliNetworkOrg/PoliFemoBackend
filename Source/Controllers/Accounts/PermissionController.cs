using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("v{version:apiVersion}/accounts/{grant}/permissions")]
[Route("/accounts/{grant}/permissions")]
public class GrantPermissionController : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPut]
    [Authorize]
    public ObjectResult GrantPermission(string grant, string idUser, long? id_object = null)
    {
        var canGrantPermissions = AuthUtil.HasPermission(AuthUtil.GetSubjectFromHttpRequest(Request),
            Constants.Permissions.PermissionsConst);
        if (!canGrantPermissions)
        {
            Response.StatusCode = 403;
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });
        }

        const string q =
            "INSERT INTO permission (id_grant, id_user, id_object) VALUES (@id_grant, @id_user, @id_object)";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>()
        {
            { "@id_grant", grant },
            { "@id_user", idUser },
            { "@id_object", id_object },
        });

        if (count > 0)
            return Ok("");

        return new BadRequestObjectResult(new JObject
        {
            { "error", "Grant failed" }
        });
    }

    [MapToApiVersion("1.0")]
    [HttpDelete]
    [Authorize]
    public ObjectResult RevokePermission(string grant, string idUser, long? id_object = null)
    {
        var canRevokePermissions = AuthUtil.HasPermission(AuthUtil.GetSubjectFromHttpRequest(Request),
            Constants.Permissions.PermissionsConst);
        if (!canRevokePermissions)
        {
            Response.StatusCode = 403;
            return new ObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });
        }

        const string q =
            "DELETE FROM  permission WHERE id_grant= @id_grant AND id_user = @id_user AND id_object = @id_object";
        var count = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>()
        {
            { "@id_grant", grant },
            { "@id_user", idUser },
            { "@id_object", id_object },
        });

        if (count > 0)
            return Ok("");

        return new BadRequestObjectResult(new JObject
        {
            { "error", "Revoke failed" }
        });
    }
}