#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

// ReSharper disable InconsistentNaming

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("v{version:apiVersion}/articles")]
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
    [MapToApiVersion("1.0")]
    [HttpPost]
    [Authorize]
    public ObjectResult InsertArticleDb(
        [FromBody] JObject data
    )
    {
        string? id_tag, title, content, subtitle, image, sourceUrl;
        DateTime? targetTime;
        int id_author;
        double latitude, longitude;
        try
        {
            id_tag = data["tag_id"]?.ToString();
            title = data["title"]?.ToString();
            subtitle = data["subtitle"]?.ToString();
            content = data["content"]?.ToString();
            targetTime = data["target_time"]?.ToObject<DateTime>();
            latitude = double.Parse(data["latitude"]?.ToString() ?? "0");
            longitude = double.Parse(data["longitude"]?.ToString() ?? "0");
            image = data["image"]?.ToString();
            id_author = int.Parse(data["author_id"]?.ToString() ?? "0");
            sourceUrl = data["source_url"]?.ToString();
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new
            {
                error = "Invalid parameters",
                message = e.Message
            });
        }

        if (id_tag == null || title == null || content == null || id_author == 0)
            return new BadRequestObjectResult(new
            {
                error = "Missing parameters"
            });

        var isValidTag = Database.ExecuteSelect($"SELECT * FROM Tags WHERE name = '{id_tag}'",
            GlobalVariables.DbConfigVar);
        if (isValidTag == null)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid tag" }
            });

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);

        if (id_author != 0)
        {
            var isValidAuthor = Database.ExecuteSelect($"SELECT * FROM Authors WHERE author_id = '{id_author}'",
                GlobalVariables.DbConfigVar);
            if (isValidAuthor == null)
                return new BadRequestObjectResult(new JObject
                {
                    { "error", "Invalid author" }
                });


            if (!AuthUtil.HasGrantAndObjectPermission(sub, "authors", id_author))
            {
                Response.StatusCode = 403;
                return new ObjectResult(new JObject
                {
                    { "error", "You don't have enough permissions" }
                });
            }
        }
        else
        {
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid author" }
            });
        }

        if ((latitude != 0 && longitude == 0) || (latitude == 0 && longitude != 0))
            return new BadRequestObjectResult(new JObject
            {
                { "error", "You must provide both latitude and longitude" }
            });
        if (latitude != 0 && (latitude is not (>= -90.0 and <= 90.0) || longitude is not (>= -180.0 and <= 180.0)))
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid latitude or longitude" }
            });

        const string insertQuery =
            @"INSERT INTO Articles(tag_id, title, subtitle, content, publish_time, target_time, latitude, longitude, image, author_id, source_url) 
            VALUES (@id_tag, @title, @subtitle, @content, NOW(), @targetTimeConverted, @latitude, @longitude, @image, @id_author, @sourceUrl)";


        var result = Database.Execute(insertQuery, GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@title", title },
                { "@content", new JArray(content).ToString(Formatting.None) },
                { "@latitude", latitude == 0 ? null : latitude },
                { "@longitude", longitude == 0 ? null : longitude },
                { "@image", image },
                { "@id_author", id_author },
                { "@sourceUrl", sourceUrl },
                { "@id_tag", id_tag },
                { "@subtitle", subtitle },
                { "@targetTimeConverted", targetTime }
            }
        );
        if (result < 0)
        {
            Response.StatusCode = 500;
            return new ObjectResult(new JObject
            {
                { "error", "Internal server error" }
            });
        }

        return Ok("");
    }
}