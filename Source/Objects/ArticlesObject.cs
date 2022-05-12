using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects;

public class ArticlesObject
{
    private readonly Dictionary<int, JToken> _articles;
    public ArticlesObject(Dictionary<int, JToken> articles)
    {
        _articles = articles;
    }

    public IEnumerable<JToken> Search(Func<KeyValuePair<int, JToken>, bool> func)
    {
        return _articles.Where(func).Select(x => x.Value).ToList();
    }

    public List<JToken>? GetArticleById(string id)
    {
        try
        {
            return new List<JToken>() { _articles[Convert.ToInt32(id)] };
        }
        catch
        {
            return null;
        }
    }
}