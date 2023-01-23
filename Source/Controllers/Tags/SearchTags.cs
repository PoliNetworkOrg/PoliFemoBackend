#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Tags;

[ApiController]
[ApiExplorerSettings(GroupName = "Tags")]
[Route("/tags")]

public class TagByIdController : ControllerBase
{
    /// <summary>
    ///     Get a list of article tags
    /// </summary>
    /// <returns>A JSON array of tags</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="404">No available tags</response>
    /// <response code="500">Can't connect to the server</response>
    
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