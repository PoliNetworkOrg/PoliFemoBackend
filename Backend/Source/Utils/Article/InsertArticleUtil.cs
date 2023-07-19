using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Articles;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Utils.Article;

public static class InsertArticleUtil
{
    internal static ObjectResult InsertArticleDbMethod(Objects.Articles.News.ArticleNews data, InsertArticle insertArticle)
    {
        var isValidTag = DB.ExecuteSelect("SELECT * FROM Tags WHERE name = @tag",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@tag", data.tag }
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


    private static ObjectResult InsertArticleDb(Objects.Articles.News.ArticleNews data, ControllerBase insertArticle)
    {
        List<int> idContent = new();

        foreach (var articlecontent in data.content) {

            if (articlecontent.title == null)
                return insertArticle.BadRequest(new JObject
                {
                    { "error", "All languages must have a valid title" }
                });

            var query = "INSERT INTO ArticleContent(title,subtitle,content,url) VALUES(@title,@subtitle,@content,@url) RETURNING id";
            var result = DB.ExecuteSelect(query, GlobalVariables.DbConfigVar,
                new Dictionary<string, object?>
                {
                    { "@title", articlecontent.title },
                    { "@subtitle", articlecontent.subtitle },
                    { "@content", articlecontent.content },
                    { "@url", null }
                });
        
            idContent.Add(Convert.ToInt32(DB.GetFirstValueFromDataTable(result)));
        }

        var insertQuery =
            @"INSERT INTO Articles(tag_id, publish_time, target_time, hidden_until, latitude, longitude, image, author_id, platforms, blurhash, content_it, content_en) 
            VALUES (@id_tag, NOW(), @targetTimeConverted, @hiddenUntil, @latitude, @longitude, @image, @id_author, @platforms, @blurhash, @ctit, @cten)";

        string? blurhash = null;
        try {
            blurhash = ArticleUtil.GenerateBlurhashAsync(data.image).Result;
        } catch (Exception) {
            return insertArticle.BadRequest(new JObject
            {
                { "error", "Invalid image" }
            });
        }

        var resultins = DB.Execute(insertQuery, GlobalVariables.DbConfigVar,
            
            new Dictionary<string, object?>
            {
                { "@latitude", data.latitude == 0 ? null : data.latitude },
                { "@longitude", data.longitude == 0 ? null : data.longitude },
                { "@image", data.image },
                { "@id_author", data.author_id },
                { "@id_tag", data.tag },
                { "@targetTimeConverted", data.target_time },
                { "@platforms", data.platforms },
                { "@hiddenUntil", data.hidden_until },
                { "@blurhash", blurhash },
                { "@ctit", idContent[0] },
                { "@cten", idContent.Count > 1 ? idContent[1] : null }
            }
        );
        if (resultins >= 0)
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