using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class Hj
{
    public List<Hj>? Children;

    public JObject? J;
    internal List<Hj>? Parents;

    public static Hj FromSingle(HtmlNode urls)
    {
        return new Hj
        {
            J = JObjectFromSingle(urls)
        };
    }

    private static JObject JObjectFromSingle(HtmlNode urls)
    {
        return new JObject
        {
            ["tag"] = urls.Name,
            ["att"] = ToJObject(urls.Attributes),
            ["text"] = urls.ChildNodes.Count == 0 ? urls.InnerText : null
        };
    }

    private static JToken ToJObject(HtmlAttributeCollection urlsAttributes)
    {
        var r = new JObject();
        foreach (var variable in urlsAttributes)
            if (variable != null)
                r[variable.Name] = variable.Value;
        return r;
    }

    // ReSharper disable once MemberCanBeMadeStatic.Global
    public void FixContent()
    {
        //todo!
    }
}