#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Articles")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
//[Authorize]
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
    /// <param name="sourceUrl">the source url?</param>
    /// <response code="200">Returns the article object</response>
    /// <response code="403">The user does not have enough permissions</response>
    /// <returns>An article object</returns>
    [MapToApiVersion("1.0")]
    [HttpPost]
    [HttpGet]
    
    public ObjectResult InsertArticleDb(string? id_tag, string title, string? subtitle, string content, DateTime? targetTime, double? latitude, double? longitude, string? image, int? id_author, string? sourceUrl) //todo: da pensare il sistema dei permessi
    {
        if(hasPermissions()){//check permission
            if(title == null|| subtitle == null)
                return new ObjectResult("handle error");
            else{
                if(id_tag != null){
                    var isValidTag = Database.ExecuteSelect($"SELECT * FROM Tags WHERE name = '{id_tag}'", GlobalVariables.DbConfigVar);
                    if(isValidTag == null)
                        return new BadRequestObjectResult("the tag provided is not valid");
                }
                if(id_author != null){
                    var isValidAuthor = Database.ExecuteSelect($"SELECT * FROM Authors WHERE id_author = '{id_author}'", GlobalVariables.DbConfigVar);
                    if(isValidAuthor == null)
                        return new BadRequestObjectResult("the author id provided is not valid");
                }

                if(latitude != null && longitude == null || latitude == null && longitude != null)
                    return new BadRequestObjectResult("location must be provided with both latitude and longitude");
                if(latitude != null && (!(latitude >= -90.0 && latitude <= 90.0) || !(longitude >= -180.0 && longitude <= 180.0)))
                    return new BadRequestObjectResult("latitude or longitude isn't valid");
                
                string publishTime = DateTime.Now.ToString("yyyy-MM-dd");
                var targetTimeConverted = targetTime == null ? "null" : targetTime.Value.ToString("yyyy-MM-dd");
                
                var insertQuery = @"INSERT INTO Articles(id_tag, title, subtitle, content, publishTime, targetTime, latitude, longitude, image, id_author, sourceUrl) 
                VALUES (@id_tag, @title, @subtitle, @content, @publishTime, @targetTimeConverted, @latitude, @longitude, @image, @id_author, @sourceUrl)";
                
                //OBBLIGATORI
                insertQuery = insertQuery.Replace("@title", $"'{title}'");
                insertQuery = insertQuery.Replace("@content", $"'{content}'");
                insertQuery = insertQuery.Replace("@publishTime", $"'{publishTime}'");
                //OPZIONALI
                insertQuery = insertQuery.Replace("@latitude", this.getStringOrNull(latitude));
                insertQuery = insertQuery.Replace("@longitude", this.getStringOrNull(longitude));
                insertQuery = insertQuery.Replace("@image", this.getStringOrNull(image));
                insertQuery = insertQuery.Replace("@id_author", this.getStringOrNull(id_author));
                insertQuery = insertQuery.Replace("@sourceUrl", this.getStringOrNull(sourceUrl));
                insertQuery = insertQuery.Replace("@id_tag", this.getStringOrNull(id_tag));
                insertQuery = insertQuery.Replace("@subtitle", this.getStringOrNull(subtitle));
                //PRECALCOLATO
                insertQuery = insertQuery.Replace("@targetTimeConverted", targetTimeConverted);
                
                
                var result = Database.Execute(insertQuery, GlobalVariables.DbConfigVar);
                if(result == -1){
                    Response.StatusCode = 500;
                    return new ObjectResult("Internal error");
                }
                else
                    return Ok(result);     
            }
        }
        else{
            Response.StatusCode = 403;
            return new ObjectResult("you don't have enough permissions");
        }
    }
    private string getStringOrNull(string? v){
        return v == null ? "null" : $"'{v}'";
    }

    private string getStringOrNull(double? v){
        return v == null ? "null" : $"{v}";
    }

    private string getStringOrNull(int? v){
        return v == null ? "null" : $"{v}";
    }

    private bool hasPermissions(){
        return true;
    }
}