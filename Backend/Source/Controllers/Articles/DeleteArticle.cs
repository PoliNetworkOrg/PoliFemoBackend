#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("/articles/{id:int}")]
public class DeleteArticle : ControllerBase
{
    /// <summary>
    ///     Remove an article
    /// </summary>
    /// <param name="id">Article ID</param>
    /// <response code="200">Request completed successfully</response>
    /// <response code="401">Authorization error</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpDelete]
    [Authorize]
    public ObjectResult DeleteArticleDb(int id)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var article = DB.ExecuteSelect("SELECT author_id from Articles WHERE article_id=@id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });
        if (article == null)
            return new NotFoundObjectResult("");
        var idAuthor = Convert.ToInt32(DB.GetFirstValueFromDataTable(article));
        if (!AccountAuthUtil.HasGrantAndObjectPermission(sub, "authors", idAuthor))
        {
            Response.StatusCode = 403;
            return new UnauthorizedObjectResult(new JObject
            {
                { "error", "You don't have enough permissions" }
            });
        }

        var result = DB.ExecuteSelect("SELECT deleteArticle(@id) as result",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", id }
            });
        if (result?.Rows[0]["result"].ToString() == "0") return Ok("");
        Response.StatusCode = 500;
        return new ObjectResult(new JObject
        {
            { "error", "Internal server error" }
        });
    }
}