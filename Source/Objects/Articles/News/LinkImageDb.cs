using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class LinkImageDb
{
    private readonly string? _alt;
    private readonly string? _innerHtml;
    internal readonly string? Src;

    public LinkImageDb(string? src, string? alt, string? innerHtml)
    {
        Src = src;
        _alt = alt;
        _innerHtml = innerHtml;
    }

    public JToken ToJson()
    {
        var obj = new JObject
        {
            ["src"] = Src,
            ["alt"] = _alt,
            ["c"] = _innerHtml
        };
        return obj;
    }
}