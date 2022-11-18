#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticlesByDateTimeRange : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    // public ObjectResult SearchArticles(DateTime? start, DateTime? end)
    // {
    //     try
    //     {
    //         var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
    //         return articlesToSearchInto == null
    //             ? ResultUtil.ExceptionResult(exception)
    //             : Ok(ArticleUtil.FilterByDateTimeRange(articlesToSearchInto, start, end));
    //     }
    //     catch (Exception ex)
    //     {
    //         return ResultUtil.ExceptionResult(ex);
    //     }
    // }
    public ObjectResult SearchArticlesByDateRange(string start, string end)
    {
        var results = Database.ExecuteSelect(
            "SELECT * FROM Articles WHERE publishTime >= @start AND publishTime <= @end",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@start", start },
                { "@end", end }
            });

        return Ok(results);
    }
}