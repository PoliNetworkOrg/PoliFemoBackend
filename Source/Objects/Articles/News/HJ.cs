using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class HJ
{
    private HtmlNode? h;

    public List<HJ>? Children;
    public List<HJ>? Parents;
    
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
            ["tag"] = urls.Name
        };
        ;
        return j;
    }

    public void FixContent()
    {
        //todo!
    }
    
    // Presa una lista di elementi con figli, parti dai figli e restituisci insieme a loro i genitori
    // r[a,b[c,d]] => [a(r),c(b,r),d(b,r)]
    public List<HJ?>? Flat()
    {
        var result = new List<HJ?>();

        if (h == null) 
            return null;
        
        result.Add(this);

        if (Children == null) 
            return result;
        
        foreach (var child in Children)
        {
            var flatChildren = child.Flat();
            if (flatChildren == null) continue;
            
            foreach (var variable in flatChildren)
            {
                if (variable != null) result.Add(variable);
            }
            result.AddRange(flatChildren);
        }

        return result;
    }
}