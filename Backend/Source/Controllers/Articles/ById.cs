#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Article;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("/articles/{id:int}")]
public class ArticleByIdController : ControllerBase
{
    /// <summary>
    ///     Search an article by ID
    /// </summary>
    /// <returns>A JSON object of article</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="404">No available articles</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SearchArticlesById(int id)
    {
        var a = SearchArticlesByIdObject(id);
        return a == null ? NotFound() : Ok(a);
    }

    private static JObject? SearchArticlesByIdObject(int id)
    {
        var results = DB.ExecuteSelect(
            "SELECT * FROM ArticlesWithAuthors_View WHERE article_id = @id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });

        var row = results?.Rows[0];
        return row == null ? null : ArticleUtil.ArticleAuthorsRowToJObject(row);
    }
}