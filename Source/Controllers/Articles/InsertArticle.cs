#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Article;

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
    public ObjectResult InsertArticleDb(
        [FromBody] ArticleNews data
    )
    {
        return InsertArticleUtil.InsertArticleDbMethod(data, this);
    }
}