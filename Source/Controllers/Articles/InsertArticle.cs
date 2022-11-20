#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
[Authorize]
public class InsertArticle : ControllerBase
{
    /// <summary>
    ///     Adds a new article to the database
    /// </summary>
    /// <param name="title">The title of the article</param>
    /// <param name="content">The content of the article</param>
    /// <response code="200">Returns the article object</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <returns>An article object</returns>
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    public ObjectResult InsertArticleDb(string? title, string? content) //todo: da pensare il sistema dei permessi
    {
        if (AuthUtil.HasPermission(AuthUtil.GetSubject(Request.Headers["Authorization"]), "pubblicare_articoli"))
        {
            var result = ArticleUtil.InsertArticle(title, content, null);
            return Ok(result);
        }

        HttpContext.Response.StatusCode = 403;
        return new ObjectResult(new
        {
            error = "Insufficient permissions",
            statusCode = 403
        });
    }
}