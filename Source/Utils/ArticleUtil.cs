#region

using System.Data;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Article;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class ArticleUtil
{
    private static Articles? _articles;
    private static readonly object LockArticles = new();


    /// <summary>
    ///     Get the articles object
    /// </summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(1)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <returns></returns>
    public static Tuple<Articles?, Exception?> GetArticles()
    {
        if (_articles != null) return new Tuple<Articles?, Exception?>(_articles, null);

        lock (LockArticles)
        {
            try
            {
                HttpClient client = new();
                using var response = client.GetAsync(Constants.ArticlesUrl).Result;
                using var content = response.Content;
                var data = content.ReadAsStringAsync().Result;
                _articles = Parse(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<Articles?, Exception?>(null, ex);
            }

            return new Tuple<Articles?, Exception?>(_articles, null);
        }
    }

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

    public static List<JToken> FilterByDateTimeRange(Articles? articlesToSearchInto, DateTime? start,
        DateTime? end)
    {
        if (start == null && end == null) return new List<JToken>();

        Func<KeyValuePair<uint, JToken>, bool> filter;
        if (start == null)
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child.Value["publishTime"]?.ToString());
                return end >= dt;
            };
        else if (end == null)
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child.Value["publishTime"]?.ToString());
                return start <= dt;
            };
        else //start and end are not null
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child.Value["publishTime"]?.ToString());
                return end >= dt && start <= dt;
            };

        var results = articlesToSearchInto?.Search(filter).ToList();
        return results ?? new List<JToken>();
    }

    public static List<JToken> FilterByTargetingTheFuture(Articles? articlesToSearchInto)
    {
        var now = DateTime.Now;
        var results = articlesToSearchInto?.Search(child =>
        {
            var dt = DateTimeUtil.ConvertToDateTime(child.Value["targetTime"]?.ToString());
            return now <= dt;
        }).ToList();
        return results ?? new List<JToken>();
    }

    public static int? InsertArticle(string? title, string? content, int? idOld)
    {
        const string q = "INSERT INTO Articles ('title', 'content', 'replace_id') VALUES (@title, @content, @rid)";
        var paramsDict = new Dictionary<string, object?>
        {
            { "@title", title },
            { "@content", content },
            { "@rid", idOld }
        };
        return Database.Database.Execute(q, GlobalVariables.DbConfigVar, paramsDict);
    }

    public static JObject ArticleAuthorsRowToJObject(DataRow row)
    {
        //convert results to json
        var a = new JObject
        {
            { "id", Convert.ToInt32(row["id_article"]) },
            { "tag_id", row["id_tag"].ToString() },
            { "title", row["title"].ToString() },
            { "subtitle", row["subtitle"].ToString() == "" ? null : row["subtitle"].ToString() },
            { "latitude", GetValue(row["latitude"]) },
            {
                "longitude", GetValue(row["longitude"])
            },
            //change format of date
            {
                "publish_time",
                DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(row["publishTime"].ToString() ?? ""))
            },
            {
                "target_time",
                DateTimeUtil.ConvertToMySqlString(DateTimeUtil.ConvertToDateTime(row["targetTime"].ToString() ?? ""))
            },
            { "content", row["content"].ToString() },
            { "image", row["image"].ToString() == "" ? null : row["image"].ToString() }
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