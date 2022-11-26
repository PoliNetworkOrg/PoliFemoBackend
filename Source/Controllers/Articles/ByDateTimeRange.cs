#region

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
[Route("v{version:apiVersion}/articles/timerange/{start}/{end}")]
[Route("/articles/timerange/{start}/{end}")]
public class ArticlesByDateTimeRange : ControllerBase
{
    /// <summary>
    ///     Search articles by time range.
    /// </summary>
    /// <remarks>
    ///     If one of the parameters is invalid, today's date will be used.
    /// </remarks>
    /// <returns>A json list of articles</returns>
    /// <response code="200">Returns articles</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available articles</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ActionResult SearchArticlesByDateRange(string? start, string? end)
    {
        var r = SearchArticlesByDateRangeAsJobject(start, end);
        return r == null ? NotFound() : Ok(r);
    }

    private static JObject? SearchArticlesByDateRangeAsJobject(string? start, string? end)
    {
        var startDateTime = DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(start) ?? DateTime.Now);
        var endDateTime = DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(end) ?? DateTime.Now);
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

        var resultsJArray = ArticleUtil.ArticleAuthorsRowsToJArray(results);

        var r = new JObject
        {
            ["results"] = resultsJArray,
            ["start"] = startDateTime,
            ["end"] = endDateTime
        };
        return r;
    }
}