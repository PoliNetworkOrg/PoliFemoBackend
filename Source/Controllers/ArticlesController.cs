using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    [HttpPost]
    public IActionResult SearchArticles(JObject payload)
    {
        HttpClient client = new();
        using HttpResponseMessage response = client.GetAsync("https://pastebin.com/raw/Giry1b7z").Result;
        using HttpContent content = response.Content;
        string data = content.ReadAsStringAsync().Result;
        try
        {
            JObject articles = JObject.Parse(data);
            List<JToken> results = Filter(payload, articles);
            return Ok(results);
        }
        catch
        {
            return Ok("Couldn't reach Github to download articles");
        }
    }

    private static List<JToken> Filter(JObject payload, JObject articles)
    {
        bool isEqual;
        List<JToken> results = new();

        foreach (JToken child in articles["articles"])
        {
            isEqual = true;
            foreach (JProperty prop in payload.Properties())
            {
                if (prop.Value.ToString() != child[prop.Name].ToString())
                {
                    isEqual = false;
                    break;
                }
            }
            if (isEqual)
            {
                results.Add(child);
            }
        }

        return results;
    }
}