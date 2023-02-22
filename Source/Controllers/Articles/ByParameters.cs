#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.DbObjects;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Article;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("/articles")]
public class ArticlesByParameters : ControllerBase
{
    /// <summary>
    ///     Search articles by parameters
    /// </summary>
    /// <param name="start" example="2022-05-18T12:15:00Z">Start time</param>
    /// <param name="end" example="2022-05-18T12:15:00Z">End time</param>
    /// <param name="tag" example="STUDENTI">Tag name</param>
    /// <param name="author_id" example="1">Author ID</param>
    /// <param name="title" example="Titolo...">Article title</param>
    /// <param name="limit" example="30">Limit of results (can be null)</param>
    /// <param name="pageOffset">Offset page for limit (can be null)</param>
    /// <param name="sort" example="date">Sort by column</param>
    /// <remarks>
    ///     At least one of the parameters must be specified.
    /// </remarks>
    /// <returns>A JSON list of articles</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    public ObjectResult SearchArticlesByDateRange(DateTime? start, DateTime? end, string? tag, int? author_id,
        string? title, uint? limit, uint? pageOffset, string? sort)
    {
        if (start == null && end == null && tag == null && author_id == null)
            return new BadRequestObjectResult(new
            {
                error = "Invalid parameters"
            });

        var r = SearchArticlesByParamsAsJobject(start, end, tag, author_id, title, new LimitOffset(limit, pageOffset),
            sort);
        return Ok(r);
    }

    private static JObject? SearchArticlesByParamsAsJobject(DateTime? start, DateTime? end, string? tag, int? author_id,
        string? title, LimitOffset limitOffset, string? sort)
    {
        var startDateTime = DateTimeUtil.ConvertToMySqlString(start ?? null);
        var endDateTime = DateTimeUtil.ConvertToMySqlString(end ?? null);
        var query = "SELECT * FROM ArticlesWithAuthors_View WHERE ";
        if (start != null) query += "publish_time >= @start AND ";
        if (end != null) query += "publish_time <= @end AND ";
        if (tag != null) query += "tag_id = @tag AND ";
        if (author_id != null) query += "author_id = @author_id AND ";
        if (title != null) query += "title LIKE @title AND ";

        query = query[..^4]; // removes last "and"

        if (sort == "date") query += "ORDER BY publish_time DESC ";

        query += limitOffset.GetLimitQuery();
        JArray resultsJArray = new();

        var results = Database.ExecuteSelect(
            query, // Remove last AND
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@start", startDateTime },
                { "@end", endDateTime },
                { "@tag", tag },
                { "@author_id", author_id },
                { "@title", "%" + title + "%" }
            });
        if (results != null)
            resultsJArray = ArticleUtil.ArticleAuthorsRowsToJArray(results);

        var r = new JObject
        {
            ["articles"] = resultsJArray,
            ["start"] = startDateTime,
            ["end"] = endDateTime,
            ["tag"] = tag,
            ["author_id"] = author_id,
            ["title"] = title
        };
        return r;
    }
}