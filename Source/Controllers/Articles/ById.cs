#region

using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======

>>>>>>> dev
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
<<<<<<< HEAD
    ///     Search article by id
=======
    ///    Search article by id
>>>>>>> dev
    /// </summary>
    /// <returns>A json of article</returns>
    /// <response code="200">Returns article</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available article</response>
<<<<<<< HEAD
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchArticlesById(int id)
    {
        Console.WriteLine(id);
=======

    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchArticlesById(int id)
    {
        Console.WriteLine(id);
        var a = SearchArticlesByIdObject(id);
        return a == null ? NotFound() : Ok(a);
    }

    public static JObject? SearchArticlesByIdObject(int id)
    {

>>>>>>> dev
        var results = Database.ExecuteSelect(
            "SELECT * FROM Articles, Authors  WHERE id_article = @id AND Articles.id_author = Authors.id_author",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });
        


        //if results is null

        var row = results?.Rows[0];
        if (row == null)
            return null;

        //convert results to json
        var a = new JObject
        {
            { "title", row["title"].ToString() },
            { "subtitle", row["subtitle"].ToString()== "" ? null : row["subtitle"].ToString()  },
            { "latitude", row["latitude"].ToString()== "" ? null : double.Parse(row["latitude"].ToString() ?? "")  },
            { "longitude",row["longitude"].ToString()== "" ? null : double.Parse(row["longituide"].ToString() ?? "")  },
            //change format of date
            { "publish_time", Utils.DateTimeUtil.ConvertToDateTime(row["publishTime"].ToString()?? "")?.ToString("yyyy-MM-dd hh:mm:ss") },
            { "target_time", Utils.DateTimeUtil.ConvertToDateTime(row["targetTime"].ToString()?? "")?.ToString("yyyy-MM-dd hh:mm:ss") },	
            { "content", row["content"].ToString() },
            { "image", row["image"].ToString() == "" ? null : row["image"].ToString()},
        };
      
        
        var b = new JObject
        {
            { "name", row["name_"].ToString() },
            { "link", row["link"].ToString() }, 
            { "image", row["image1"].ToString()},
        };

        a.Add("author", b);

<<<<<<< HEAD

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
=======
        return a;
>>>>>>> dev
    }
}