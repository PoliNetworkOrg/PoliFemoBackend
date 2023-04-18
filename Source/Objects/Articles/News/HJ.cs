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
    
    // Presa una lista di elementi con figli, parti dai figli e restituisci insieme a loro i genitori
    // r[a,b[c,d]] => [a(r),c(b,r),d(b,r)]
    public List<Hj?>? Flat()
    {
        var result = new List<Hj?>();

        if (this.Children == null || this.Children.Count == 0)
            return new List<Hj?>() { this };

        foreach (var variable in this.Children)
        {
            Flat2(new List<Hj?>() { this }, variable, result);
        }

        return result;
    }

    private static void Flat2(List<Hj?> list, Hj variable, ICollection<Hj?> result)
    {
        if (variable.Children == null || variable.Children.Count == 0)
        {
            variable.Parents ??= new List<Hj>();
            foreach (var v in list)
            {
                if (v != null) 
                    variable.Parents.Add(v);
            }
            result.Add(variable);
            return;
        }

        //se ha dei figli estraimoli
        List<Hj?> parentList = new List<Hj?>();
        foreach (var v in list)
        {
            if (v != null) 
                parentList.Add(v);
        }
        parentList.Add(variable);
        foreach (var child in variable.Children)
        {
            Flat2(parentList, child, result);
        }
    }
}