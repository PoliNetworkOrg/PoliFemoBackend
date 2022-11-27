#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;



#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class DeleteArticle : ControllerBase
{
    /// <summary>
    ///     Removes article from database
    /// </summary>
    /// <param name="id_article">id of the article's to delete</param>
    /// <response code="200">Article deleted successfully</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    [Authorize]
    public ObjectResult DeleteArticleDb(int id_article)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var article = Database.ExecuteSelect($"SELECT id_author from Articles WHERE id_article={id_article}",GlobalVariables.DbConfigVar);
        if(article == null)
            return new BadRequestObjectResult(new JObject
            {
                { "error","no articles found with specified id" }
            });
        var id_author = Convert.ToInt32(Database.GetFirstValueFromDataTable(article));
        if(!AuthUtil.HasGrantAndObjectPermission(sub, "autori", id_author)){
            Response.StatusCode = 403;
            return new BadRequestObjectResult(new JObject
            {
                { "error","you don't have enough permissions" }
            });
        }
        var result = Database.Execute($"DELETE FROM Articles WHERE id_article={id_article}",GlobalVariables.DbConfigVar);
        if(result == -1){
            Response.StatusCode = 500;
            return new BadRequestObjectResult(new JObject
            {
                { "error","internal server error" }
            });
        }
        return Ok("");
    }


  
}