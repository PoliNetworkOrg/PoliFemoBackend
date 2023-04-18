#region

using System.Data;
using Blurhash.ImageSharp;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Articles;

#endregion

namespace PoliFemoBackend.Source.Utils.Article;

public static class ArticleUtil
{
    /// <summary>
    /// </summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(n)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <param name="data"></param>
    /// <returns></returns>
    private static Articles? Parse(string data)
    {
        var parsed = JObject.Parse(data);
        var articles = parsed["articles"];
        if (articles == null) return null;

        var result = new Dictionary<uint, JToken>();
        foreach (var child in articles) result[Convert.ToUInt32(child["id"])] = child;

        return new Articles(result);
    }

    public static async Task<string?> GenerateBlurhashAsync(string? url)
    {
        if (string.IsNullOrEmpty(url)) return null;

        await using var bytes = await new HttpClient().GetStreamAsync(url);
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
            { "title", row["title"].ToString() },
            { "subtitle", row["subtitle"].ToString() == "" ? null : row["subtitle"].ToString() },
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
            { "content", JArray.Parse(row["content"].ToString() ?? "") },
            { "image", row["image"].ToString() == "" ? null : row["image"].ToString() },
            { "blurhash", row["blurhash"].ToString() == "" ? null : row["blurhash"].ToString() }
        };


        var b = new JObject
        {
            { "name", row["author_name"].ToString() },
            { "link", row["author_link"].ToString() },
            { "image", row["author_image"].ToString() }
        };

        a.Add("author", b);
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