#region

using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

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
        if (results == null) return NotFound();

        //convert results to json
        
        var a = new JObject
        {
            { "title", results.Rows[0]["title"].ToString() },
            { "subtitle", results.Rows[0]["subtitle"].ToString()== "" ? null : results.Rows[0]["subtitle"].ToString()  },
            { "latitude", results.Rows[0]["latitude"].ToString()== "" ? null : Double.Parse(results.Rows[0]["latitude"].ToString() ?? "")  },
            { "longitude", results.Rows[0]["longitude"].ToString()== "" ? null : Double.Parse(results.Rows[0]["longituide"].ToString() ?? "")  },
            //change format of date
            { "publish_time", DateTime.Parse(results.Rows[0]["publishTime"].ToString()?? "").ToString("yyyy-MM-dd hh:mm:ss") },
            { "target_time", DateTime.Parse(results.Rows[0]["targetTime"].ToString()?? "").ToString("yyyy-MM-dd hh:mm:ss") },	
            { "content", results.Rows[0]["content"].ToString() },
            { "image", results.Rows[0]["image"].ToString() == "" ? null : results.Rows[0]["image"].ToString()},
        };

        if(results.Rows[0]["targetTime"].ToString() == "")
            a.Add("target_time", null);
        
        var b = new JObject
        {
            { "name", results.Rows[0]["name_"].ToString() },
            { "link", results.Rows[0]["link"].ToString() }, 
            { "image", results.Rows[0]["image1"].ToString()},
        };

        a.Add("author", b);
        
        return Ok(a);
        
    }
}