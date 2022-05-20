using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects;

public class ArticlesObject
{
    private readonly Dictionary<uint, JToken> _articles; // indexed by id
    private readonly Dictionary<string, List<JToken>> _articlesByAuthor; // indexed by author

    public ArticlesObject(Dictionary<uint, JToken> articles)
    {
        _articles = articles;
        _articlesByAuthor = FillAuthors(_articles);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <complexity>
    ///     <best>O(n)</best>
    ///     <average>O(n)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <param name="articles"></param>
    /// <returns></returns>
    private static Dictionary<string, List<JToken>> FillAuthors(Dictionary<uint, JToken> articles)
    {
        var result = new Dictionary<string, List<JToken>>();
        foreach (var article in articles)
        {
            var author = article.Value["author"]?.ToString();
            if (!string.IsNullOrEmpty(author))
            {
                if (!result.ContainsKey(author))
                {
                    result.Add(author, new List<JToken>());
                }
                result[author].Add(article.Value);
            }
        }
        return result;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(1)</average>
    ///     <worst>O(1)</worst>
    /// </complexity>
    /// <param name="author"></param>
    /// <returns></returns>
    public List<JToken> FilterByAuthor(string author)
    {
        return _articlesByAuthor[author];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <complexity>
    ///     <best>O(n)</best>
    ///     <average>O(n)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <param name="func"></param>
    /// <returns></returns>
    public IEnumerable<JToken> Search(Func<KeyValuePair<uint, JToken>, bool> func)
    {
        return _articles.Where(func).Select(x => x.Value).ToList();
    }

    public List<JToken> GetArticleById(uint id)
    {
        return _articles.Where(x => x.Key == id).Select(x => x.Value).ToList();
    }
    
    /// <summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(10)</average> // 10 is the number of new articles that we expect the client is missing on average
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<JToken> FilterByStartingId(uint id)
    {
        if (id == 0) {
            return new List<JToken>();
        }
        
        try
        {
            var results = new List<JToken>();
            for (var i = id; i <= _articles.Count; i++)
            {
                results.Add(_articles[i]);
            }
            return results ?? new List<JToken>();
        }
        catch
        {
            return new List<JToken>();
        }
    }
}