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
[Route("v{version:apiVersion}/articles/{id}")]
[Route("/articles/{id}")]
public class ArticleByIdController : ControllerBase
{
  
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
        if (results == null) return GroupsUtil.ErrorInRetrievingGroups();

        if (results.Rows.Count == 0) return NotFound();

        //convert results to json
        var a = new JObject();
        a.Add("title", results.Rows[0]["title"].ToString());
        a.Add("subtitle", results.Rows[0]["subtitle"].ToString());
        a.Add("publishTime", results.Rows[0]["publishTime"].ToString());
        a.Add("targetTime", results.Rows[0]["targetTime"].ToString());
        a.Add("content", results.Rows[0]["content"].ToString());
        var b = new JObject();
       
        b.Add("name", results.Rows[0]["name_"].ToString());
        b.Add("image", results.Rows[0]["image"].ToString());
        b.Add("link", results.Rows[0]["link"].ToString());
        a.Add("author", b);
        
        return Ok(a);
        
    }
}