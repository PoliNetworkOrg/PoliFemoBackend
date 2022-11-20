#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Tags;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Tags")]
[Route("v{version:apiVersion}/tags")]
[Route("/tags")]

public class TagByIdController : ControllerBase
{
    /// <summary>
    ///     Search Tags
    /// </summary>
    /// <returns>A json of tags</returns>
    /// <response code="200">Returns the array of tags</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available tags</response>

    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchTags()
    {
        
        var results = Database.ExecuteSelect(
            "SELECT * FROM Tags",
            GlobalVariables.DbConfigVar);


        //if results is null
        if (results == null) return StatusCode(500);  

        if (results.Rows.Count == 0) return NotFound();

        //convert results to json
        var a = new JObject { { "tags", HandleDataUtil.GetResultsAsJArray(results) } };

        return Ok(a);
        
    }
}