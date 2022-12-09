#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class InsertArticleOverAnOldOne : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    public ObjectResult
        InsertArticleOverAnOldOneDb(int idOld, string? title, string? content) //todo: auth + all parameters
    {
        const string q1 = "SELECT * FROM Articles WHERE id_article = @rid";
        var paramsDict1 = new Dictionary<string, object?>
        {
            { "@rid", idOld }
        };
        var result1 = Database.ExecuteSelect(q1, GlobalVariables.DbConfigVar, paramsDict1);
        if (result1 == null || result1.Rows.Count <= 0)
            return new BadRequestObjectResult("Id of the old article is invalid. No article found.");

        const string q2 = "SELECT * FROM Articles WHERE replace_id = @rid";
        var paramsDict2 = new Dictionary<string, object?>
        {
            { "@rid", idOld }
        };
        var result2 = Database.ExecuteSelect(q2, GlobalVariables.DbConfigVar, paramsDict2);
        if (result2 is { Rows.Count: > 0 })
            return new BadRequestObjectResult(
                "Id of the old article is invalid. That article has been already overwritten.");

        var result3 = ArticleUtil.InsertArticle(title, content, idOld);
        return Ok(result3);
    }
}