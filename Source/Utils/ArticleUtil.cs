#region

using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class ArticleUtil
{
    private static ArticlesObject? _articles;
    private static readonly object LockArticles = new();



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

    private static ArticlesObject? Parse(string data)
    {
        var parsed = JObject.Parse(data);
        var articles = parsed["articles"];
        if (articles == null)
            return null;
        
        var result = new Dictionary<int, JToken>();
        foreach (var child in articles)
        {
            result[Convert.ToInt32(child["id"])] = child;
        }

        return new ArticlesObject(result);
    }

    internal static List<JToken> FilterById(ArticlesObject? articlesToSearchInto, int id)
    {
        return articlesToSearchInto?.GetArticleById(id) ?? new List<JToken>();
    }

    public static List<JToken> FilterByAuthor(ArticlesObject? articlesToSearchInto, string? author)
    {
        if (string.IsNullOrEmpty(author))
            return new List<JToken>();

        var results = articlesToSearchInto?.Search(child =>
        {
            var b = child.Value["authors"]?.ToList().Any(x => x.ToString().Contains(author));
            return b != null && b.Value;
        }).ToList();
        return results ?? new List<JToken>();
    }

    public static List<JToken> FilterByDateTimeRange(ArticlesObject? articlesToSearchInto, DateTime? start, DateTime? end)
    {
        if (start == null && end == null)
            return new List<JToken>();

        Func<KeyValuePair<int, JToken>, bool> filter;
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

    public static List<JToken> FilterByStartingId(ArticlesObject? articlesToSearchInto, int id)
    {
        try
        {
            var results = articlesToSearchInto?.Search(
                child => Convert.ToInt32(child.Value["id"]?.ToString()) >= id
            ).ToList();
            return results ?? new List<JToken>();
        }
        catch
        {
            return new List<JToken>();
        }
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