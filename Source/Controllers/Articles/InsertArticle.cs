#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class InsertArticle : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    public ObjectResult InsertArticleDb(string? title, string? content) //todo: auth + all parameters
    {
        var result = ArticleUtil.InsertArticle(title, content, null);
        return Ok(result);
    }
}