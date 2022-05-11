﻿#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private static JObject? _articles;

    [HttpGet]
    public ObjectResult SearchArticles(string? id, string? author)
    {
        try
        {
            var (articlesToSearchInto, exception) = GetArticles();
            return articlesToSearchInto == null
                ? ErrorFindingArticles(exception)
                : Ok(Filter(articlesToSearchInto, id, author));
        }
        catch (Exception ex)
        {
            return ErrorFindingArticles(ex);
        }
    }

    private static ObjectResult ErrorFindingArticles(Exception? ex)
    {
        ObjectResult objectResult = new(null)
        {
            StatusCode = 500,
            Value = ex?.Message ?? ""
        };
        return objectResult;
    }

    private static Tuple<JObject?, Exception?> GetArticles()
    {
        if (_articles != null)
            return new Tuple<JObject?, Exception?>(_articles, null);
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

    private static List<JToken> Filter(JObject articles, string? id, string? author)
    {
        Func<JToken, bool> idCheck;
        if (string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(author))
            idCheck = child => child["id"]?.ToString() == id;
        else if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(author))
            idCheck = _ => true;
        else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(author))
            idCheck = child =>
            {
                var b = child["authors"]?.ToList().Any(x => Match(x, author));
                return b != null && b.Value;
            };
        else if (string.IsNullOrEmpty(id) == false && !string.IsNullOrEmpty(author))
            idCheck = child =>
            {
                var b = child["authors"]?.ToList().Any(x => Match(x, author));
                var b2 = child["id"]?.ToString() == id;
                return b != null && b.Value && b2;
            };
        else
            idCheck = _ => false;

        var results = articles["articles"]?.Where(idCheck).ToList();
        return results ?? new List<JToken>();
    }

    private static bool Match(JToken x, string author)
    {
        return x.ToString().Contains(author);
    }
}