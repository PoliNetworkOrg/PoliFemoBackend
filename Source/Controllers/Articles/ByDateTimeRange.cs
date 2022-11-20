#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;


[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("v{version:apiVersion}/articles/timerange/{start}/{end}")]
[Route("/articles/timerange/{start}/{end}")]
public class ArticlesByDateTimeRange : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchArticlesByDateRange(string? start, string? end)
    {
        var r = SearchArticlesByDateRangeAsJobject(start, end);
        return r == null ? NotFound() : Ok(r);
    }

    public static JObject? SearchArticlesByDateRangeAsJobject(string? start, string? end)
    {
        var startDateTime = Utils.DateTimeUtil.ConvertToMySqlString(Utils.DateTimeUtil.ConvertToDateTime(start) ?? DateTime.Now);
        var endDateTime = Utils.DateTimeUtil.ConvertToMySqlString(Utils.DateTimeUtil.ConvertToDateTime(end) ?? DateTime.Now);
        var results = Database.ExecuteSelect(
            "SELECT * FROM ArticlesWithAuthors_View WHERE publishTime >= '@start' AND publishTime <= '@end'",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@start", startDateTime },
                { "@end", endDateTime }
            });

        if (results == null || results.Rows.Count == 0)
            return null;

        var resultsJArray = Utils.ArticleUtil.ArticleAuthorsRowsToJArray(results);

        var r = new JObject
        {
            ["result"] = resultsJArray,
            ["start"] = startDateTime,
            ["end"] = endDateTime
        };
        return r;
    }
}