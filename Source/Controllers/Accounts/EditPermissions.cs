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
[Route("v{version:apiVersion}/accounts/{id}/permissions")]
[Route("/accounts/{id}/permissions")]
public class EditPermissions : ControllerBase
{
    /// <summary>
    ///     Edits the permissions of a user
    /// </summary>
    /// <remarks>
    ///     The body is an array of objects with the following structure:
    ///     - grant: String
    ///     - object_id: Integer
    /// </remarks>
    /// <param name="id">The id of the user</param>
    /// <param name="data">The array of permissions</param>
    /// <response code="200">Permissions updated successfully</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="400">A received grant is not valid</response>
    [MapToApiVersion("1.0")]
    [HttpPut]
    [Authorize]
    public ObjectResult EditPermissionsController(string id, [FromBody] JArray data)
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

        var q = "DELETE FROM permission WHERE id_user = @id_user";
        Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>
        {
            { "@id_user", id }
        }); 

        foreach (var grant in data)
        {
            var idGrant = grant["grant"]?.ToString();
            var idObject = grant["object_id"]?.ToString();
            if (idGrant == null || idObject == null)
                return new BadRequestObjectResult(new JObject
                {
                    { "error", "Invalid request" }
                });
            
            q = "INSERT INTO permission (id_user, id_grant, id_object) VALUES (@id_user, @id_grant, @id_object)";
            try {
                var r = Database.Execute(q, DbConfig.DbConfigVar, new Dictionary<string, object?>
                {
                    { "@id_user", id },
                    { "@id_grant", idGrant },
                    { "@id_object", idObject == "" ? null : idObject }
                });
            } catch (Exception) {
                return new BadRequestObjectResult(new JObject
                {
                    { "error", "An error occurred while updating the permissions, make sure the grants are valid" }
                });
            }


        }
        return Ok("");
    }
}