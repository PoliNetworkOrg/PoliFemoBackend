#region

using System.Data;
using Blurhash.ImageSharp;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Articles;
using Image = SixLabors.ImageSharp.Image;

#endregion

namespace PoliFemoBackend.Source.Utils.Article;

public static class ArticleUtil
{
    public static async Task<string?> GenerateBlurhashAsync(string? url)
    {
        if (url == null || url == "") return null;

        using(var bytes = await new HttpClient().GetStreamAsync(url)) {
            var image = Image.Load<Rgba32>(bytes);
            return Blurhasher.Encode(image, 5, 5);
        }
    }

    public static JObject ArticleAuthorsRowToJObject(DataRow row)
    {
        //convert results to json
        var a = new JObject
        {
            { "id", Convert.ToInt32(row["article_id"]) },
            { "tag_id", row["tag_id"].ToString() },
            { "latitude", GetValue(row["latitude"]) },
            {
                "longitude", GetValue(row["longitude"])
            },
            //change format of date
            {
                "publish_time",
                DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(row["publish_time"].ToString() ?? ""))
            },
            {
                "target_time",
                DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(row["target_time"].ToString() ?? ""))
            },
            {
                "hidden_until",
                DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(row["hidden_until"].ToString() ?? ""))
            },
            { "image", row["article_image"].ToString() == "" ? null : row["article_image"].ToString() },
            { "blurhash", row["blurhash"].ToString() == "" ? null : row["blurhash"].ToString() }
        };

        var contit = new JObject
            {
                { "title", row["title_it"].ToString() },
                { "subtitle", row["subtitle_it"].ToString() != "" ? row["subtitle_it"].ToString() : null },
                { "content", row["content_it"].ToString() },
                { "url", row["url_it"].ToString() != "" ? row["url_it"].ToString() : null}
            };

        var conten = row["title_en"].ToString() != "" ? new JObject
            {
                { "title", row["title_en"].ToString() },
                { "subtitle", row["subtitle_en"].ToString() != "" ? row["subtitle_en"].ToString() : null },
                { "content", row["content_en"].ToString() },
                { "url", row["url_en"].ToString() != "" ? row["url_en"].ToString() : null }
            } : null;


        var b = new JObject
        {
            { "name", row["author_name"].ToString() },
            { "link", row["author_link"].ToString() },
            { "image", row["author_image"].ToString() }
        };

        var content = new JObject
        {
            { "it", contit },
            { "en", conten }
        };

        a.Add("author", b);
        a.Add("content", content);
        return a;
    }

    private static JToken? GetValue(object? o)
    {
        return o == null ? null : new JValue(o);
    }

    public static JArray ArticleAuthorsRowsToJArray(DataTable results)
    {
        var r = new JArray();
        foreach (DataRow dr in results.Rows) r.Add(ArticleAuthorsRowToJObject(dr));
        return r;
    }
}