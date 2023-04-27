using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Articles;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;

namespace PoliFemoBackend.Source.Utils.Article;

public static class InsertArticleUtil
{
    internal static ObjectResult InsertArticleDbMethod(Objects.Articles.News.Article data, InsertArticle insertArticle)
    {
        var isValidTag = Database.Database.ExecuteSelect("SELECT * FROM Tags WHERE name = @tag",
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

        var sub = AuthUtil.GetSubjectFromHttpRequest(insertArticle.Request);

        var errorCheckAuthor = CheckAuthorUtil.CheckAuthourErros(data, insertArticle, sub);
        if (errorCheckAuthor != null)
            return errorCheckAuthor;

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
        if (data.platforms is < 0 or > 3)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid platforms" }
            });

        return InsertArticleDb(data, insertArticle);
    }


    private static ObjectResult InsertArticleDb(Objects.Articles.News.Article data, ControllerBase insertArticle)
    {
        const string insertQuery =
            @"INSERT INTO Articles(tag_id, title, subtitle, content, publish_time, target_time, latitude, longitude, image, author_id, source_url, platforms, hidden_until, blurhash) 
            VALUES (@id_tag, @title, @subtitle, @content, NOW(), @targetTimeConverted, @latitude, @longitude, @image, @id_author, null, @platforms, @hiddenUntil, @blurhash)";


        var result = Database.Database.Execute(insertQuery, GlobalVariables.DbConfigVar,
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
                { "@targetTimeConverted", data.target_time },
                { "@platforms", data.platforms },
                { "@hiddenUntil", data.hidden_until },
                { "@blurhash", ArticleUtil.GenerateBlurhashAsync(data.image).Result }
            }
        );
        if (result >= 0)
            return insertArticle.Created("", new JObject
            {
                { "message", "Article created successfully" }
            });

        insertArticle.Response.StatusCode = 500;
        return new ObjectResult(new JObject
        {
            { "error", "Internal server error" }
        });
    }
}