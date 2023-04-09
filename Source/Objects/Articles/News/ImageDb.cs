using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ImageDb
{
    private readonly string? _alt;
    internal readonly string? Src;

    public ImageDb(string? src, string? alt)
    {
        Src = src;
        _alt = alt;
    }

    public JToken ToJson()
    {
        var obj = new JObject
        {
            ["src"] = Src,
            ["alt"] = _alt
        };
        return obj;
    }
}