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
    ///     Adds a new article to the database
    /// </summary>
    /// <remarks>
    ///     All parameters must be passed in the body of the request formatted as a JSON object.
    ///     The following parameters are required:
    ///     - title: String
    ///     - content: String
    ///     - id_author: Integer
    ///     - id_tag: String
    ///     <br />
    ///     <br />
    ///     The following parameters are optional:
    ///     - subtitle: String
    ///     - image: String
    ///     - target_time: DateTime
    ///     - latitude: Double
    ///     - longitude: Double
    /// </remarks>
    /// <response code="200">Article inserted successfully</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to server</response>
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
        try {
            id_tag = data["id_tag"]?.ToString();
            title = data["title"]?.ToString();
            subtitle = data["subtitle"]?.ToString();
            content = data["content"]?.ToString();
            targetTime = data["target_time"]?.ToObject<DateTime>();
            latitude = Double.Parse(data["latitude"]?.ToString() ?? "0");
            longitude = Double.Parse(data["longitude"]?.ToString() ?? "0");
            image = data["image"]?.ToString();
            id_author = Int32.Parse(data["id_author"]?.ToString() ?? "0");
            sourceUrl = data["sourceUrl"]?.ToString();
        } catch (Exception e) {
            return new BadRequestObjectResult(new
            {
                error = "Invalid parameters",
                message = e.Message
            });
        }

        if (id_tag == null || title == null || subtitle == null || content == null || id_author == 0)
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
            var isValidAuthor = Database.ExecuteSelect($"SELECT * FROM Authors WHERE id_author = '{id_author}'",
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
        } else {
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

        var publishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var targetTimeConverted = targetTime == null ? "null" : targetTime.Value.ToString("yyyy-MM-dd HH:mm:ss");

        var insertQuery =
            @"INSERT INTO Articles(id_tag, title, subtitle, content, publishTime, targetTime, latitude, longitude, image, id_author, sourceUrl) 
            VALUES (@id_tag, @title, @subtitle, @content, @publishTime, @targetTimeConverted, @latitude, @longitude, @image, @id_author, @sourceUrl)";

        var contentArray = Utils.ArticleUtil.EncodeStringList(new List<string>() { content });


        var result = Database.Execute(insertQuery, GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>()
            {
                {"@title", GetStringOrNull(title)},
                {"@content", "'" + JsonConvert.SerializeObject(contentArray) + "'"},
                {"@publishTime", GetStringOrNull(publishTime)},
                {"@latitude", latitude == 0 ? "null" : $"'{latitude}'"},
                {"@longitude", longitude == 0 ? "null" : $"'{longitude}'"},
                {"@image", GetStringOrNull(image)},
                {"@id_author", GetStringOrNull(id_author)},
                {"@sourceUrl", GetStringOrNull(sourceUrl)},
                {"@id_tag", GetStringOrNull(id_tag)},
                {"@subtitle", GetStringOrNull(subtitle)},
                {"@targetTimeConverted", targetTimeConverted}
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


    private static string GetStringOrNull(string? v)
    {
        return v == null ? "null" : $"'{v}'";
    }

    private static string GetStringOrNull(double? v)
    {
        return v == null ? "null" : $"{v}";
    }

    private static string GetStringOrNull(int? v)
    {
        return v == null ? "null" : $"{v}";
    }
}