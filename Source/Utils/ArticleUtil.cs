#region

using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class ArticleUtil
{
    private static ArticlesObject? _articles;
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
    public static Tuple<ArticlesObject?, Exception?> GetArticles()
    {
        if (_articles != null)
            return new Tuple<ArticlesObject?, Exception?>(_articles, null);

        lock (LockArticles)
        {
            try
            {
                HttpClient client = new();
                using var response = client.GetAsync(Constants.Constants.ArticlesUrl).Result;
                using var content = response.Content;
                var data = content.ReadAsStringAsync().Result;
                _articles = Parse(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<ArticlesObject?, Exception?>(null, ex);
            }

            return new Tuple<ArticlesObject?, Exception?>(_articles, null);
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
    private static ArticlesObject? Parse(string data)
    {
        var parsed = JObject.Parse(data);
        var articles = parsed["articles"];
        if (articles == null)
            return null;

        var result = new Dictionary<uint, JToken>();
        foreach (var child in articles) result[Convert.ToUInt32(child["id"])] = child;

        return new ArticlesObject(result);
    }

    public static List<JToken> FilterByDateTimeRange(ArticlesObject? articlesToSearchInto, DateTime? start,
        DateTime? end)
    {
        if (start == null && end == null)
            return new List<JToken>();

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

    public static List<JToken> FilterByTargetingTheFuture(ArticlesObject? articlesToSearchInto)
    {
        var now = DateTime.Now;
        var results = articlesToSearchInto?.Search(child =>
        {
            var dt = DateTimeUtil.ConvertToDateTime(child.Value["targetTime"]?.ToString());
            return now <= dt;
        }).ToList();
        return results ?? new List<JToken>();
    }
}