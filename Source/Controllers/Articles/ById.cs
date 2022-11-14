#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("v{version:apiVersion}/articles/{id:int}")]
[Route("/articles/{id:int}")]
public class ArticleByIdController : ControllerBase
{
    /// <summary>
    ///    Search article by id
    /// </summary>
    /// <returns>A json of article</returns>
    /// <response code="200">Returns article</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available article</response>

    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchArticlesById(int id)
    {
        Console.WriteLine(id);
        var results = Database.ExecuteSelect(
            "SELECT * FROM Articles, Authors  WHERE id_article = @id AND Articles.id_author = Authors.id_author",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });


        //if results is null
        if (results == null) return StatusCode(500);

        if (results.Rows.Count == 0) return NotFound();

        //convert results to json
        var a = new JObject
        {
            { "title", results.Rows[0]["title"].ToString() },
            { "subtitle", results.Rows[0]["subtitle"].ToString() },
            { "publishTime", results.Rows[0]["publishTime"].ToString() },
            { "targetTime", results.Rows[0]["targetTime"].ToString() },
            { "content", results.Rows[0]["content"].ToString() }
        };
        var b = new JObject
        {
            { "name", results.Rows[0]["name_"].ToString() },
            { "image", results.Rows[0]["image"].ToString() },
            { "link", results.Rows[0]["link"].ToString() }
        };

        a.Add("author", b);
        
        return Ok(a);
        
    }
}