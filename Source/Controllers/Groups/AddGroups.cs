#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Groups;
using PoliFemoBackend.Source.Utils.Database;
using PoliFemoBackend.Source.Utils.Groups.AddGroup;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[Obsolete("This endpoint will be removed with no replacement.")]
[ApiController]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("/groups")]
public class AddGroupsController : ControllerBase
{
    /// <summary>
    ///     Add a new group
    /// </summary>
    /// <returns>An array of Group objects</returns>
    /// <response code="200">Request completed succesfully</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [Authorize]
    [HttpPost]
    public ObjectResult AddGroupsDb(Group group)
    {
        var d = new Dictionary<string, object?> { { "@name", group.name } };

        var query = BuildQueryUtil.BuildQuery(group, d);

        var results = Database.Execute(query, GlobalVariables.DbConfigVar, d);

        return results == 0
            ? StatusCode(500, new { message = "Can't connect to server" })
            : new CreatedResult("/groups", new { message = "Group added", status = 200 });
    }
}