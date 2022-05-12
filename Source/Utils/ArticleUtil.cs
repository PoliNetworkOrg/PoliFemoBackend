#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class ArticleUtil
{
    private static JObject? _articles;
    private static readonly object LockArticles = new();

    public static ObjectResult ErrorFindingArticles(Exception? ex)
    {
        ObjectResult objectResult = new(null)
        {
            StatusCode = 500,
            Value = ex?.Message ?? ""
        };
        return objectResult;
    }

    public static Tuple<JObject?, Exception?> GetArticles()
    {
        if (_articles != null)
            return new Tuple<JObject?, Exception?>(_articles, null);

        lock (LockArticles)
        {
            try
            {
                HttpClient client = new();
                using var response = client.GetAsync(Constants.Constants.ArticlesUrl).Result;
                using var content = response.Content;
                var data = content.ReadAsStringAsync().Result;
                _articles = JObject.Parse(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Tuple<JObject?, Exception?>(null, ex);
            }

            return new Tuple<JObject?, Exception?>(_articles, null);
        }
    }

    internal static List<JToken> FilterById(JObject articlesToSearchInto, string id)
    {
        if (string.IsNullOrEmpty(id))
            return new List<JToken>();

        var results = articlesToSearchInto["articles"]?.Where(child => child["id"]?.ToString() == id).ToList();
        return results ?? new List<JToken>();
    }

    public static List<JToken> FilterByAuthor(JObject articlesToSearchInto, string? author)
    {
        if (string.IsNullOrEmpty(author))
            return new List<JToken>();

        var results = articlesToSearchInto["articles"]?.Where(child =>
        {
            var b = child["authors"]?.ToList().Any(x => x.ToString().Contains(author));
            return b != null && b.Value;
        }).ToList();
        return results ?? new List<JToken>();
    }

    public static List<JToken> FilterByDateTimeRange(JObject articlesToSearchInto, DateTime? start, DateTime? end)
    {
        if (start == null && end == null)
            return new List<JToken>();

        Func<JToken, bool> filter;
        if (start == null)
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child["publishTime"]?.ToString());
                return end >= dt;
            };
        else if (end == null)
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child["publishTime"]?.ToString());
                return start <= dt;
            };
        else //start and end are not null
            filter = child =>
            {
                var dt = DateTimeUtil.ConvertToDateTime(child["publishTime"]?.ToString());
                return end >= dt && start <= dt;
            };

        var results = articlesToSearchInto["articles"]?.Where(filter).ToList();
        return results ?? new List<JToken>();
    }

    public static List<JToken> FilterByStartingId(JObject articlesToSearchInto, string id)
    {
        if (string.IsNullOrEmpty(id))
            return new List<JToken>();

        try
        {
            var idInt = Convert.ToInt32(id);
            var results = articlesToSearchInto["articles"]?.Where(
                child => Convert.ToInt32(child["id"]?.ToString()) >= idInt
            ).ToList();
            return results ?? new List<JToken>();
        }
        catch
        {
            return new List<JToken>();
        }
    }
}