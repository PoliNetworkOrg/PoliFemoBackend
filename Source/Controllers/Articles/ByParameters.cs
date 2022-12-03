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
public class ArticlesByParameters : ControllerBase
{
    /// <summary>
    ///     Search articles by parameters.
    /// </summary>
    /// <param name="start" example="2022-05-18T12:15:00Z">Start time</param>
    /// <param name="end" example="2022-05-18T12:15:00Z">End time</param>
    /// <param name="tag" example="STUDENTI">Tag name</param>
    /// <param name="author_id" example="1">Author id</param>
    /// <param name="title" example="Titolo...">Article title</param>
    /// <param name="limit" example="30">Limit of results</param>
    /// <remarks>
    ///     At least one of the parameters must be specified.
    /// </remarks>
    /// <returns>A json list of articles</returns>
    /// <response code="200">Articles found</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="404">No available articles</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    public ObjectResult SearchArticlesByDateRange(DateTime? start, DateTime? end, string? tag, int? author_id, string? title, int? limit)
    {
        if (start == null && end == null && tag == null && author_id == null)
        {
            return new BadRequestObjectResult(new
            {
                error = "Invalid parameters"
            });
        }

        var r = SearchArticlesByParamsAsJobject(start, end, tag, author_id, title, limit);
        return r == null ? new NotFoundObjectResult("") : Ok(r);
    }

    private static JObject? SearchArticlesByParamsAsJobject(DateTime? start, DateTime? end, string? tag, int? author_id, string? title, int? limit)
    {
        var startDateTime = DateTimeUtil.ConvertToMySqlString(start ?? null);
        var endDateTime = DateTimeUtil.ConvertToMySqlString(end ?? null);
        var query = "SELECT * FROM ArticlesWithAuthors_View WHERE ";
        if (start != null) {
            query += "publishTime >= @start AND ";
        }
        if (end != null) {
            query += "publishTime <= @end AND ";
        }
        if (tag != null) {
            query += "id_tag = @tag AND ";
        }
        if (author_id != null) {
            query += "id_author = @author_id AND ";
        }
        if (title != null) {
            query += "title LIKE @title AND ";
        }

        query = query.Substring(0, query.Length - 4);
        query += "LIMIT " + ((limit == null || limit < 1 || limit > 100) ? 30 : limit);
        var results = Database.ExecuteSelect(
            query,  // Remove last AND
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@start", startDateTime },
                { "@end", endDateTime },
                { "@tag", tag },
                { "@author_id", author_id },
                { "@title", "%" + title + "%" }
            });
        if (results == null || results.Rows.Count == 0)
            return null;

        var resultsJArray = ArticleUtil.ArticleAuthorsRowsToJArray(results);

        var r = new JObject
        {
            ["results"] = resultsJArray,
            ["start"] = startDateTime,
            ["end"] = endDateTime,
            ["tag"] = tag,
            ["author_id"] = author_id,
            ["title"] = title
        };
        return r;
    }
}