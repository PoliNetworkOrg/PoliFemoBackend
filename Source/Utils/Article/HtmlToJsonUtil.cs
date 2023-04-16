using HtmlAgilityPack;
using Jsonize;
using Jsonize.Parser;
using Jsonize.Serializer;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Article;

public static class HtmlToJsonUtil
{
    public static string? GetContentFromHtml(HtmlNodeCollection urls1)
    {
        List<HtmlNode>? urls = null;
        try
        {
            var x = urls1.First(x => x.GetClasses().Contains("news-single-item"));
            urls = new List<HtmlNode>() { x };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        try
        {
            var x = urls1.Where(x => x.GetClasses().Contains("container") && !x.GetClasses().Contains("frame-type-header"));
            urls = x.ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        

        return urls != null ? H(urls) : null;
    }

 

    private static string? H(List<HtmlNode> urls)
    {

        var hj = GetHj(urls);
        hj.FixContent();
        var list = hj.Flat();
        var j = GetJArray(list);
        if (j == null)
            return null;
        var s = Newtonsoft.Json.JsonConvert.SerializeObject(j);
        return s;
    }

    private static JArray? GetJArray(List<Hj?>? list)
    {
        ;
        ;
        try
        {
            var r = new JArray();
            if (list == null)
                return null;

            foreach (var variable in list)
            {
                var variableJ = variable?.J;

                if (variableJ != null)
                {
                    variableJ["parents"] = GetParents(variable);
                    r.Add(variableJ);
                }
            }

            return r;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        ;
        ;
        return null;
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

    private static Hj GetHj(List<HtmlNode> urls)
    {
        if (urls.Count == 0)
        {
            var v = urls[0];
            var hj2 = Hj.FromSingle(v);
            foreach (var variable in v.ChildNodes)
            {
                hj2.Children ??= new List<Hj>();
                hj2.Children.Add(GetHj(new List<HtmlNode>(){variable}));
            }

            return hj2;
        }
        
        var result = new Hj();

        foreach (var v in urls)
        {
            var hj2 = Hj.FromSingle(v);
            foreach (var variable in v.ChildNodes)
            {
                hj2.Children ??= new List<Hj>();
                hj2.Children.Add(GetHj(new List<HtmlNode>(){variable}));
            }

            result.Children ??= new List<Hj>();
            result.Children.Add(hj2);
        }

        return result;
    }

    private static string? S(HtmlNode urls)
    {
        try
        {
            var jsonizeParser = new JsonizeParser();
            var jsonizeSerializer = new JsonizeSerializer();
            var jsonizer = new Jsonizer(jsonizeParser, jsonizeSerializer);

            var c = jsonizer.ParseToStringAsync(urls.InnerHtml).Result;
            return c;
        }
        catch
        {
            // ignored
        }

        return null;
    }
}