#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
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
    ///     Search article by id
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
        var a = SearchArticlesByIdObject(id);
        return a == null ? NotFound() : Ok(a);
    }

    private static JObject? SearchArticlesByIdObject(int id)
    {
        var results = Database.ExecuteSelect(
            "SELECT * FROM ArticlesWithAuthors_View  WHERE id_article = @id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });
        
        var row = results?.Rows[0];
        return row == null ? null : ArticleUtil.ArticleAuthorsRowToJObject(row);
    }
}