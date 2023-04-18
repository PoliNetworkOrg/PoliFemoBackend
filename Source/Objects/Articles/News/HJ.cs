using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class Hj
{
    //private HtmlNode? _h;

    public List<Hj>? Children;
    internal List<Hj>? Parents;
    
    public JObject? J;

    public static Hj FromSingle(HtmlNode urls)
    {
        var r = new Hj
        {
            //_h = urls,
            J = JObjectFromSingle(urls)
        };
        return r;
    }

    private static JObject JObjectFromSingle(HtmlNode urls)
    {
        var j = new JObject
        {
            ["tag"] = urls.Name,
            ["att"] = ToJObject(urls.Attributes),
            ["text"] = urls.ChildNodes.Count == 0 ? urls.InnerText : null
        };
        ;
        return j;
    }

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