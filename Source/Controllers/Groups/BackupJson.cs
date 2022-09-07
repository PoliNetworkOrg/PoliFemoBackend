#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("v{version:apiVersion}/groups/backup")]
[Route("/groups/backup")]
public class BackupGroupsController : ControllerBase
{
    /// <summary>
    ///     Backup of groups
    /// </summary>
    /// <returns>A json of groups</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [MapToApiVersion("1.0")]
    [HttpGet]

    public ActionResult BackupGroupsDb()
    {
        var query = "SELECT * FROM Groups ORDER BY class";
        var results = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, null);
        
        if (results == null)
            return StatusCode(500, "Can't connect to server");
        
        if (results.Rows.Count == 0){
            return NoContent();
        }

        var json = JsonConvert.SerializeObject(results, Formatting.Indented);

        var groups = new JObject();
        groups.Add("gruppi",json );
        return Ok(groups);
        
        
    }
}