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
[Route("v{version:apiVersion}/articles")]
[Route("/articles")]
public class ArticlesByDateTimeRange : ControllerBase
{
    /// <summary>
    ///     Search articles by time range.
    /// </summary>
    /// <param name="start">Start time</param>
    /// <param name="end">End time</param>
    /// <param name="tag">Tag name</param>
    /// <param name="limit">Limit of results</param>
    /// <remarks>
    ///     At least one of the parameters must be specified.
    /// </remarks>
    /// <returns>A json list of articles</returns>
    /// <response code="200">Articles found</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available articles</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult SearchArticlesByDateRange(string? start, string? end, string? tag, int? limit)
    {
        if (start == null && end == null && tag == null)
        {
            return new BadRequestObjectResult(new
            {
                error = "Invalid parameters"
            });
        }

        var r = SearchArticlesByParamsAsJobject(start, end, tag, limit);
        return r == null ? new NotFoundObjectResult("") : Ok(r);
    }

    private static JObject? SearchArticlesByParamsAsJobject(string? start, string? end, string? tag, int? limit)
    {
        var startDateTime = DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(start) ?? null);
        var endDateTime = DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(end) ?? null);
        var query = "SELECT * FROM ArticlesWithAuthors_View WHERE "; //rifare la view
        if (start != null) {
            query += "publishTime >= @start AND ";
        }
        if (end != null) {
            query += "publishTime <= @end AND ";
        }
        if (tag != null) {
            query += "id_tag = @tag AND ";
        }

        query = query.Substring(0, query.Length - 4);
        query += " LIMIT " + (limit ?? 30);

        var results = Database.ExecuteSelect(
            query.Substring(0, query.Length - 4),  // Remove last AND
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@start", startDateTime },
                { "@end", endDateTime },
                { "@tag", tag }
            });

        if (results == null || results.Rows.Count == 0)
            return null;

        var resultsJArray = ArticleUtil.ArticleAuthorsRowsToJArray(results);

        var r = new JObject
        {
            ["results"] = resultsJArray,
            ["start"] = startDateTime,
            ["end"] = endDateTime,
            ["tag"] = tag
        };
        return r;
    }
}