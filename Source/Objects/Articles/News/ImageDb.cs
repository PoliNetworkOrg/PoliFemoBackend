using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ImageDb
{
    internal readonly string Src;
    private readonly string _alt;

    public ImageDb(string src, string alt)
    {
        this.Src = src;
        this._alt = alt;
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