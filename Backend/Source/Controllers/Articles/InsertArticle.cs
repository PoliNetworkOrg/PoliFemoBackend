#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Article;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("/articles")]
public class InsertArticle : ControllerBase
{
    /// <summary>
    ///     Add a new article
    /// </summary>
    /// <remarks>
    ///     All parameters must be passed in the body of the request formatted as a JSON object.
    ///     The following parameters are required:
    ///     - title: String
    ///     - content: String
    ///     - author_id: Integer
    ///     - tag_id: String
    ///     <br />
    ///     <br />
    ///     The following parameters are optional:
    ///     - subtitle: String
    ///     - image: String
    ///     - target_time: DateTime
    ///     - latitude: Double
    ///     - longitude: Double
    /// </remarks>
    /// <response code="200">Request completed successfully</response>
    /// <response code="401">Authorization error</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpPost]
    [Authorize]
    public ObjectResult InsertArticleDb([FromBody] ArticleNews data)
    {
        var isValidTag = DB.ExecuteSelect(
            "SELECT * FROM Tags WHERE name = @tag",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@tag", data.tag } }
        );
        if (isValidTag == null)
            return new BadRequestObjectResult(new JObject { { "error", "Invalid tag" } });

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);

        var errorCheckAuthor = ArticleUtil.CheckAuthorErrors(data, this, sub);
        if (errorCheckAuthor != null)
            return errorCheckAuthor;

        if (
            (data.latitude != 0 && data.longitude == 0)
            || (data.latitude == 0 && data.longitude != 0)
        )
            return new BadRequestObjectResult(
                new JObject { { "error", "You must provide both latitude and longitude" } }
            );
        if (
            data.latitude != 0
            && (data.latitude is < -90 or > 90 || data.longitude is < -180 or > 180)
        )
            return new BadRequestObjectResult(
                new JObject { { "error", "Invalid latitude or longitude" } }
            );
        if (data.platforms is < 0 or > 3)
            return new BadRequestObjectResult(new JObject { { "error", "Invalid platforms" } });

        return ArticleUtil.InsertArticleDb(data, this);
    }
}
