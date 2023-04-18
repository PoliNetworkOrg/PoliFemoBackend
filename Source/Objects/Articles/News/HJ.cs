using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class Hj
{
    public List<Hj>? Children;
    internal List<Hj>? Parents;
    
    public JObject? J;

    public static Hj FromSingle(HtmlNode urls) =>
        new()
        {
            J = JObjectFromSingle(urls)
        };

    private static JObject JObjectFromSingle(HtmlNode urls) =>
        new()
        {
            ["tag"] = urls.Name,
            ["att"] = ToJObject(urls.Attributes),
            ["text"] = urls.ChildNodes.Count == 0 ? urls.InnerText : null
        };

    private static JToken ToJObject(HtmlAttributeCollection urlsAttributes)
    {
        var r = new JObject();
        foreach (var variable in urlsAttributes)
        {
            if (variable != null)
            {
                r[variable.Name] = variable.Value;
            }
        }
        return r;
    }

    public void FixContent()
    {
        //todo!
    }
    
}