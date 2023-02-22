#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils.Groups.ModifyGroup;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[Obsolete("This endpoint will be removed with no replacement.")]
[ApiController]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("/groups/{id}")]
public class ModifyGroupsController : ControllerBase
{
    /// <summary>
    ///     Modify group
    /// </summary>
    /// <param name="ob">New group proprieties</param>
    /// <param name="id">Group ID</param>
    /// <returns>An array of Group objects</returns>
    /// <response code="200">Request completed succesfully</response>
    /// <response code="500">Can't connect to server</response>
    [HttpPut]
    public ObjectResult ModifyGroupsDb(JObject ob, string id)
    {
        return ModifyGroupUtil.ModifyGroupMethod(ob, id, this);
    }
}