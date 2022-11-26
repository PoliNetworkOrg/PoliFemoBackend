#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils.Database;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
    /// <param name="id_tag">id of the article's tag, must be a valid tag</param>
    /// <param name="title">title of the article</param>
    /// <param name="subtitle">subtitle of the article</param>
    /// <param name="content">text content of the article</param>
    /// <param name="targetTime">optional time target of the article if it's an event</param>
    /// <param name="latitude">latitude of the event's position, must be provided with longitude</param>
    /// <param name="longitude">longitude of the event's position, must be provided with latitude</param>
    /// <param name="image">the image url</param>
    /// <param name="id_author">the author's id, must be a valid id</param>
    /// <param name="sourceUrl">the source url (only for polimi news)</param>
    /// <response code="200">Article inserted successfully</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [HttpPost]
    [Authorize]
    public ObjectResult InsertArticleDb(
        string? id_tag, string title, string? subtitle, [FromBody] string content, DateTime? targetTime,
        double? latitude, double? longitude, string? image, int? id_author, string? sourceUrl
    )
    {
        if (id_tag != null)
        {
            var isValidTag = Database.ExecuteSelect($"SELECT * FROM Tags WHERE name = '{id_tag}'", GlobalVariables.DbConfigVar);
            if (isValidTag == null)
                return new BadRequestObjectResult(new JObject
                {
                    {"error", "Invalid tag"}
                });
        }

        var sub = AuthUtil.GetSubjectFromHttpRequest(this.Request);

        if (id_author != null)
        {
            var isValidAuthor = Database.ExecuteSelect($"SELECT * FROM Authors WHERE id_author = '{id_author}'", GlobalVariables.DbConfigVar);
            if (isValidAuthor == null)
                return new BadRequestObjectResult(new JObject()
                {
                    {"error", "Invalid author"}
                });


            if (!AuthUtil.HasGrantAndObjectPermission(sub, "autori", id_author.Value)) {
                Response.StatusCode = 403;
                return new ObjectResult(new JObject()
                {
                    {"error", "You don't have enough permissions"}
                });
            }
        }

        if (latitude != null && longitude == null || latitude == null && longitude != null)
            return new BadRequestObjectResult(new JObject()
            {
                {"error", "You must provide both latitude and longitude"}
            });
        if (latitude != null && (latitude is not (>= -90.0 and <= 90.0) || longitude is not (>= -180.0 and <= 180.0)))
            return new BadRequestObjectResult(new JObject()
            {
                {"error", "Invalid latitude or longitude"}
            });

        string publishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var targetTimeConverted = targetTime == null ? "null" : targetTime.Value.ToString("yyyy-MM-dd HH:mm:ss");

        var insertQuery = @"INSERT INTO Articles(id_tag, title, subtitle, content, publishTime, targetTime, latitude, longitude, image, id_author, sourceUrl) 
            VALUES (@id_tag, @title, @subtitle, @content, @publishTime, @targetTimeConverted, @latitude, @longitude, @image, @id_author, @sourceUrl)";

        var conarray = new JArray();
        conarray.Add(content);

        //OBBLIGATORI
        insertQuery = insertQuery.Replace("@title", $"'{title}'");
        insertQuery = insertQuery.Replace("@content", $"'{JsonConvert.SerializeObject(conarray)}'");
        insertQuery = insertQuery.Replace("@publishTime", $"'{publishTime}'");
        //OPZIONALI
        insertQuery = insertQuery.Replace("@latitude", GetStringOrNull(latitude));
        insertQuery = insertQuery.Replace("@longitude", GetStringOrNull(longitude));
        insertQuery = insertQuery.Replace("@image", GetStringOrNull(image));
        insertQuery = insertQuery.Replace("@id_author", GetStringOrNull(id_author));
        insertQuery = insertQuery.Replace("@sourceUrl", GetStringOrNull(sourceUrl));
        insertQuery = insertQuery.Replace("@id_tag", GetStringOrNull(id_tag));
        insertQuery = insertQuery.Replace("@subtitle", GetStringOrNull(subtitle));
        //PRECALCOLATO
        insertQuery = insertQuery.Replace("@targetTimeConverted", targetTimeConverted);


        var result = Database.Execute(insertQuery, GlobalVariables.DbConfigVar);
        if (result == -1)
        {
            Response.StatusCode = 500;
            return new ObjectResult(new JObject()
            {
                {"error", "Internal server error"}
            });
        }
        else
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