﻿using HtmlAgilityPack;
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
        var urls = urls1.First(x => x.GetClasses().Contains("news-single-item"));
        if (urls != null)
        {
            return H(urls);
            return S(urls);
        }

        return null;
    }

 

    private static string? H(HtmlNode urls)
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

    private static JArray? GetJArray(List<HJ?>? list)
    {
        var r = new JArray();
        if (list == null) 
            return null;
        
        foreach (var VARIABLE in list)
        {
            var variableJ = VARIABLE?.j;
            if (variableJ != null) 
                r.Add(variableJ);
        }

        return r;
    }

    private static HJ GetHj(HtmlNode urls)
    {
        var hj2 = HJ.FromSingle(urls);
        foreach (var variable in urls.ChildNodes)
        {
            hj2.Children ??= new List<HJ>();
            hj2.Children.Add(GetHj(variable));
        }
        return hj2;
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