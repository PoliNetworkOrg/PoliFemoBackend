using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Article;

public static class HtmlToJsonUtil
{
    private static readonly Func<HtmlNode, bool> News1 = x => x.GetClasses().Contains("news-single-item");
    private static readonly Func<HtmlNode, bool> News2 = x => x.GetClasses().Contains("container") && !x.GetClasses().Contains("frame-type-header");
    public static string? GetContentFromHtml(HtmlNodeCollection urls1)
    {
        List<HtmlNode>? urls = null;
        try
        {
            var x = urls1.First(News1);
            urls = new List<HtmlNode>() { x };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        try
        {
            var x = urls1.Where(News2);
            urls = x.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        
        return urls != null ? H(urls) : null;
    }

 

    private static string? H(IReadOnlyList<HtmlNode> urls)
    {
        var j = H2(urls);
        return j == null ? null : Newtonsoft.Json.JsonConvert.SerializeObject(j);
    }

    private static JArray? H2(IReadOnlyList<HtmlNode> urls)
    {
        var hj = GetHj(urls);
        hj.FixContent();
        var list = Html.Flat.FlatUtil.Flat(hj);
        return GetJArray(list);
    }

    private static JArray? GetJArray(List<Hj?>? list)
    {
        try
        {
            var r = new JArray();
            if (list == null)
                return null;

            foreach (var variable in list)
            {
                AddHjToJArray(variable, r);
            }

            return r;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return null;
    }

    private static void AddHjToJArray(Hj? variable, JArray r)
    {
        var variableJ = variable?.J;

        if (variableJ == null) return;
        variableJ["parents"] = GetParents(variable);
        r.Add(variableJ);
    }

    private static JArray GetParents(Hj? variableJ)
    {
        var j = new JArray();
        var variableJParents = variableJ?.Parents;
        if (variableJParents == null) 
            return j;
        
        foreach (var singleParent in variableJParents)
        {
            var jObject = singleParent.J;
            if (jObject != null) 
                j.Add(jObject);
        }
        return j;
    }

    private static Hj GetHj(IReadOnlyList<HtmlNode> urls)
    {
        return urls.Count == 0 ? GetHjSingle(urls) : GetHjList(urls);
    }

    private static Hj GetHjList(IEnumerable<HtmlNode> urls)
    {
        var result = new Hj();
        foreach (var hj2 in urls.Select(GetHjSingle2))
        {
            result.Children ??= new List<Hj>();
            result.Children.Add(hj2);
        }

        return result;
    }
    
    private static Hj GetHjSingle2(HtmlNode v)
    {
        var hj2 = Hj.FromSingle(v);
        foreach (var variable in v.ChildNodes)
        {
            hj2.Children ??= new List<Hj>();
            hj2.Children.Add(GetHj(new List<HtmlNode> { variable }));
        }

        return hj2;
    }

    private static Hj GetHjSingle(IReadOnlyList<HtmlNode> urls)
    {
        var v = urls[0];
        var hj2 = GetHjSingle2(v);

        return hj2;
    }


}