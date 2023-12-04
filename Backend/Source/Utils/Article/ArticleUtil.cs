#region

using System.Data;
using Blurhash.ImageSharp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Articles;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Auth;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Article;

public static class ArticleUtil
{
    public static async Task<string?> GenerateBlurhashAsync(string? url)
    {
        if (url == null || url == "")
            return null;

        using var bytes = await new HttpClient().GetStreamAsync(url);
        var image = Image.Load<Rgba32>(bytes);
        return Blurhasher.Encode(image, 5, 5);
    }

    public static JObject ArticleAuthorsRowToJObject(DataRow row)
    {
        //convert results to json
        var a = new JObject
        {
            { "id", Convert.ToInt32(row["article_id"]) },
            { "tag_id", row["tag_id"].ToString() },
            { "latitude", GetValue(row["latitude"]) },
            { "longitude", GetValue(row["longitude"]) },
            //change format of date
            {
                "publish_time",
                DateTimeUtil.ConvertToMySqlString(
                    DateTimeUtil.ConvertToDateTime(row["publish_time"].ToString() ?? "")
                )
            },
            {
                "target_time",
                DateTimeUtil.ConvertToMySqlString(
                    DateTimeUtil.ConvertToDateTime(row["target_time"].ToString() ?? "")
                )
            },
            {
                "hidden_until",
                DateTimeUtil.ConvertToMySqlString(
                    DateTimeUtil.ConvertToDateTime(row["hidden_until"].ToString() ?? "")
                )
            },
            {
                "image",
                row["article_image"].ToString() == "" ? null : row["article_image"].ToString()
            },
            { "blurhash", row["blurhash"].ToString() == "" ? null : row["blurhash"].ToString() }
        };

        var contit = new JObject
        {
            { "title", row["title_it"].ToString() },
            {
                "subtitle",
                row["subtitle_it"].ToString() != "" ? row["subtitle_it"].ToString() : null
            },
            { "content", row["content_it"].ToString() },
            { "url", row["url_it"].ToString() != "" ? row["url_it"].ToString() : null }
        };

        var conten =
            row["title_en"].ToString() != ""
                ? new JObject
                {
                    { "title", row["title_en"].ToString() },
                    {
                        "subtitle",
                        row["subtitle_en"].ToString() != "" ? row["subtitle_en"].ToString() : null
                    },
                    { "content", row["content_en"].ToString() },
                    { "url", row["url_en"].ToString() != "" ? row["url_en"].ToString() : null }
                }
                : null;

        var b = new JObject
        {
            { "name", row["author_name"].ToString() },
            { "link", row["author_link"].ToString() },
            { "image", row["author_image"].ToString() }
        };

        var content = new JObject { { "it", contit }, { "en", conten } };

        a.Add("author", b);
        a.Add("content", content);
        return a;
    }

    private static JToken? GetValue(object? o)
    {
        return o == null ? null : new JValue(o);
    }

    public static ObjectResult InsertArticleDb(ArticleNews data, ControllerBase insertArticle)
    {
        List<int> idContent = new();

        foreach (var articlecontent in data.content)
        {
            if (articlecontent.title == null)
                return insertArticle.BadRequest(
                    new JObject { { "error", "All languages must have a valid title" } }
                );

            var query =
                "INSERT INTO ArticleContent(title,subtitle,content,url) VALUES(@title,@subtitle,@content,@url) RETURNING id";
            var result = DB.ExecuteSelect(
                query,
                GlobalVariables.DbConfigVar,
                new Dictionary<string, object?>
                {
                    { "@title", articlecontent.title },
                    { "@subtitle", articlecontent.subtitle },
                    { "@content", articlecontent.content },
                    { "@url", null }
                }
            );

            idContent.Add(Convert.ToInt32(DB.GetFirstValueFromDataTable(result)));
        }

        var insertQuery =
            @"INSERT INTO Articles(tag_id, publish_time, target_time, hidden_until, latitude, longitude, image, author_id, platforms, blurhash, content_it, content_en) 
            VALUES (@id_tag, NOW(), @targetTimeConverted, @hiddenUntil, @latitude, @longitude, @image, @id_author, @platforms, @blurhash, @ctit, @cten)";

        string? blurhash;
        try
        {
            blurhash = GenerateBlurhashAsync(data.image).Result;
        }
        catch (Exception)
        {
            return insertArticle.BadRequest(new JObject { { "error", "Invalid image" } });
        }

        var resultins = DB.Execute(
            insertQuery,
            GlobalVariables.DbConfigVar,
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
            return insertArticle.Created(
                "",
                new JObject { { "message", "Article created successfully" } }
            );

        insertArticle.Response.StatusCode = 500;
        return new ObjectResult(new JObject { { "error", "Internal server error" } });
    }

    internal static ObjectResult? CheckAuthorErrors(
        ArticleNews data,
        InsertArticle insertArticle,
        string? sub
    )
    {
        if (data.author_id == 0)
            return new BadRequestObjectResult(new JObject { { "error", "Invalid author" } });

        var isValidAuthor = DB.ExecuteSelect(
            "SELECT * FROM Authors WHERE author_id = @id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?> { { "@id", data.author_id } }
        );
        if (isValidAuthor == null)
            return new BadRequestObjectResult(new JObject { { "error", "Invalid author" } });

        if (
            AccountAuthUtil.HasGrantAndObjectPermission(
                sub,
                Constants.Permissions.ManageArticles,
                data.author_id
            )
        )
            return null;

        insertArticle.Response.StatusCode = 403;
        return new ObjectResult(new JObject { { "error", "You don't have enough permissions" } });
    }
}
