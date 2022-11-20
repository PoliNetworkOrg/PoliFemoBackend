#region

using Microsoft.AspNetCore.Mvc;
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
    [HttpPost]
    public ObjectResult SearchArticlesByDateRange(string start, string end)
    {
        var startDateTime = Utils.DateTimeUtil.ConvertToDateTime(start) ?? DateTime.Now;
        var endDateTime = Utils.DateTimeUtil.ConvertToDateTime(end) ?? DateTime.Now;
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