using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Articles;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Utils.Article;

public static class CheckAuthorUtil
{
    /// <summary>
    ///     Get author errors (if any)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="insertArticle"></param>
    /// <param name="sub"></param>
    /// <returns>if there is an error, return it, null otherwise</returns>
    internal static ObjectResult? CheckAuthourErros(ArticleNews data, InsertArticle insertArticle,
        string? sub)
    {
        if (data.author_id == 0)
            return new BadRequestObjectResult(new JObject
            {
                { "error", "Invalid author" }
            });

        var isValidAuthor = DB.ExecuteSelect("SELECT * FROM Authors WHERE author_id = @id",
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

        if (AccountAuthUtil.HasGrantAndObjectPermission(sub, "authors", data.author_id))
            return null;

        insertArticle.Response.StatusCode = 403;
        return new ObjectResult(new JObject
        {
            { "error", "You don't have enough permissions" }
        });
    }
}