using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class InsertArticle :ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    public ObjectResult InsertArticleDb(string title, string content) //todo: auth + all parameters
    {
        const string q = "INSERT INTO Articles ('title', 'content') VALUES (@title, @content)";
        var paramsDict = new Dictionary<string, object>()
        {
            {"@title", title},
            {"@content", content}
        };
        var result = Source.Utils.Database.Execute(q, GlobalVariables.DbConfigVar, paramsDict);
        return Ok(result);
    }
}