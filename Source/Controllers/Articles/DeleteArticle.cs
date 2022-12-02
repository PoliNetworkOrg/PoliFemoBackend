#region

using Microsoft.AspNetCore.Authorization;
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
public class DeleteArticle : ControllerBase
{
    /// <summary>
    ///     Removes an article from database
    /// </summary>
    /// <param name="id">ID of the article to be deleted</param>
    /// <response code="200">Article deleted successfully</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpDelete]
    [Authorize]
    public ObjectResult DeleteArticleDb(int id)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var article = Database.ExecuteSelect("SELECT id_author from Articles WHERE id_article=@id",
        GlobalVariables.DbConfigVar,
        new Dictionary<string, object?>
        {
            {"@id", id}
        });
        if(article == null)
            return new NotFoundObjectResult("");
        var id_author = Convert.ToInt32(Database.GetFirstValueFromDataTable(article));
        if(!AuthUtil.HasGrantAndObjectPermission(sub, "authors", id_author)){
            Response.StatusCode = 403;
            return new UnauthorizedObjectResult(new JObject
            {
                { "error","You don't have enough permissions" }
            });
        }
        var result = Database.Execute("DELETE FROM Articles WHERE id_article=@id",
        GlobalVariables.DbConfigVar,
        new Dictionary<string, object?>
        {
            {"@id", id}
        });
        if(result < 0){
            Response.StatusCode = 500;
            return new ObjectResult(new JObject
            {
                { "error","Internal server error" }
            });
        }
        return Ok("");
    }


  
}