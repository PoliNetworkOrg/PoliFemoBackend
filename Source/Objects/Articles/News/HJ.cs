using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class HJ
{
    private HtmlNode? h;

    public List<HJ>? Children;
    internal List<HJ>? Parents;
    
    public JObject? j;

    public static HJ FromSingle(HtmlNode urls)
    {
        var r = new HJ
        {
            h = urls,
            j = JObjectFromSingle(urls)
        };
        return r;
    }

    private static JObject JObjectFromSingle(HtmlNode urls)
    {

        
        JObject j = new JObject
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
    public List<HJ?>? Flat()
    {
        List<HJ?> result = new List<HJ?>();

        if (this.Children == null || this.Children.Count == 0)
            return new List<HJ?>() { this };

        foreach (var variable in this.Children)
        {
            Flat2(new List<HJ?>() { this }, variable, result);
        }

        return result;
    }

    private static void Flat2(List<HJ?> list, HJ variable, ICollection<HJ?> result)
    {
        if (variable.Children == null || variable.Children.Count == 0)
        {
            variable.Parents ??= new List<HJ>();
            foreach (var v in list)
            {
                if (v != null) 
                    variable.Parents.Add(v);
            }
            result.Add(variable);
            return;
        }

        //se ha dei figli estraimoli
        List<HJ?> parentList = new List<HJ?>();
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