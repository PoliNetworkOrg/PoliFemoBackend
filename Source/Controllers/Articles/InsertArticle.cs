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
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    public ObjectResult InsertArticleDb(string? title, string? content) //todo: auth + all parameters
    {
        if (!AuthUtil.CanInsertArticles(Request)) return Unauthorized("User can't insert articles.");

        var result = ArticleUtil.InsertArticle(title, content, null);
        return Ok(result);
    }
}