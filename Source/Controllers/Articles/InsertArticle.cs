#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Articles;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

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
        [FromBody] Article data
    )
    {
        var isValidTag = Database.ExecuteSelect("SELECT * FROM Tags WHERE name = @tag",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@tag", data.tag_id }
            });
        if (isValidTag == null)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid tag" }
            });

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);

        if (data.author_id != 0)
        {
            var isValidAuthor = Database.ExecuteSelect("SELECT * FROM Authors WHERE author_id = @id",
                GlobalVariables.DbConfigVar,
                new Dictionary<string, object?>
                {
                    { "@id", data.author_id }
                });
            if (isValidAuthor == null)
                return new BadRequestObjectResult(new JObject
                {
                    { "error", "Invalid author" }
                });


            if (!AuthUtil.HasGrantAndObjectPermission(sub, "authors", data.author_id))
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

        if ((data.latitude != 0 && data.longitude == 0) || (data.latitude == 0 && data.longitude != 0))
            return new BadRequestObjectResult(new JObject
            {
                { "error", "You must provide both latitude and longitude" }
            });
        if (data.latitude != 0 &&
            (data.latitude is < -90 or > 90 || data.longitude is < -180 or > 180))
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid latitude or longitude" }
            });

        const string insertQuery =
            @"INSERT INTO Articles(tag_id, title, subtitle, content, publish_time, target_time, latitude, longitude, image, author_id, source_url) 
            VALUES (@id_tag, @title, @subtitle, @content, NOW(), @targetTimeConverted, @latitude, @longitude, @image, @id_author, null)";


        var result = Database.Execute(insertQuery, GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@title", data.title },
                { "@content", new JArray(data.content).ToString(Formatting.None) },
                { "@latitude", data.latitude == 0 ? null : data.latitude },
                { "@longitude", data.longitude == 0 ? null : data.longitude },
                { "@image", data.image },
                { "@id_author", data.author_id },
                { "@id_tag", data.tag_id },
                { "@subtitle", data.subtitle },
                { "@targetTimeConverted", data.target_time }
            }
        );
        if (result >= 0)
            return Created("", new JObject
            {
                { "message", "Article created successfully" }
            });

        Response.StatusCode = 500;
        return new ObjectResult(new JObject
        {
            { "error", "Internal server error" }
        });
    }
}