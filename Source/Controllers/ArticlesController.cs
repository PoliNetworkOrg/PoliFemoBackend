using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace PoliFemoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    [HttpGet]
    public ObjectResult SearchArticles(string? id, string? author)
    {
        HttpClient client = new();
        using HttpResponseMessage response = client.GetAsync("https://pastebin.com/raw/Giry1b7z").Result;
        using HttpContent content = response.Content;
        string data = content.ReadAsStringAsync().Result;
        try
        {
            JObject articles = JObject.Parse(data);
            List<JToken> results = Filter(articles, id, author);
            return Ok(results);
        }
        catch (Exception ex)
        {
            ObjectResult objectResult = new(null)
            {
                StatusCode = 500,
                Value = ex.Message
            };
            return objectResult;
        }
    }

    private static List<JToken> Filter(JObject articles, string? id, string? author)
    {
        Func<JToken, bool> idCheck = null;
        if (string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(author))
            idCheck = (child) => { return child["id"]?.ToString() == id; };
        else if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(author))
            idCheck = (child) => { return true; };
        else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(author))
        {
            idCheck = (child) => { 
                var b = child["authors"]?.ToList().Any(x => Match(x, author));
                return b != null && b.Value;
            };
        }
        else if (string.IsNullOrEmpty(id) == false && !(string.IsNullOrEmpty(author)))
        {
            idCheck = (child) =>
            {
                var b = child["authors"]?.ToList().Any(x => Match(x, author));
                var b2 = child["id"]?.ToString() == id;
                return b != null && b.Value && b2;
            };
        }
        else
        {
            idCheck = (child) => { return false; };
        }

        var results = articles["articles"]?.Where(idCheck).ToList();
        return results ?? new List<JToken>();
    }

    private static bool Match(JToken x, string author)
    {
        return x.ToString().Contains(author);
    }
}