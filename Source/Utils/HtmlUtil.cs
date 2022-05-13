﻿using System.Net;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects;

namespace PoliFemoBackend.Source.Utils;

public static class HtmlUtil
{
    internal static async Task<WebReply> DownloadHtmlAsync(string urlAddress)

    {
        HttpClient httpClient = new();
        var response = await httpClient.GetAsync(urlAddress);

        if (response.StatusCode != HttpStatusCode.OK)
            return new WebReply(null, response.StatusCode);

        var receiveStream = response.Content;
        try
        {
            var s = await receiveStream.ReadAsStringAsync();

            return new WebReply(s, HttpStatusCode.OK);
        }
        catch
        {
            return new WebReply(null, HttpStatusCode.ExpectationFailed);
        }
    }
    
    internal static List<HtmlNode>? GetElementsByTagAndClassName(HtmlNode? doc, string tag = "",
        string className = "", long? limit = null)
    {
        if (doc == null)
            return null;

        var lst = new List<HtmlNode>();
        var emptyTag = string.IsNullOrEmpty(tag);
        var emptyCn = string.IsNullOrEmpty(className);
        if (emptyTag && emptyCn) return null;

        if (limit is <= 0)
            return null;

        var result = new List<HtmlNode>();

        if (emptyTag && limit == null)
        {
            lst.Add(doc);
            for (var i = 0; i < lst.Count; i++)
            {
                if (lst[i].GetClasses().Contains(className)) result.Add(lst[i]);

                var childcollection = lst[i].ChildNodes;
                if (childcollection == null) continue;
                lst.AddRange(childcollection);
            }

            return result;
        }

        switch (emptyCn)
        {
            case true when limit == null:
            {
                lst.Add(doc);
                for (var i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Name == tag) result.Add(lst[i]);

                    var childcollection = lst[i].ChildNodes;
                    if (childcollection == null) continue;
                    lst.AddRange(childcollection);
                }

                return result;
            }
            case false when emptyTag == false && limit == null:
            {
                lst.Add(doc);
                for (var i = 0; i < lst.Count; i++)
                {
                    if (lst[i].GetClasses().Contains(className) && lst[i].Name == tag) result.Add(lst[i]);

                    var childcollection = lst[i].ChildNodes;
                    if (childcollection == null) continue;
                    lst.AddRange(childcollection);
                }

                return result;
            }
        }

        if (emptyTag && limit != null)
        {
            lst.Add(doc);
            for (var i = 0; i < lst.Count; i++)
            {
                if (lst[i].GetClasses().Contains(className))
                {
                    result.Add(lst[i]);

                    if (result.Count == limit.Value)
                        return result;
                }

                var childcollection = lst[i].ChildNodes;
                if (childcollection == null) continue;
                lst.AddRange(childcollection);
            }

            return result;
        }

        switch (emptyCn)
        {
            case true when limit != null:
            {
                lst.Add(doc);
                for (var i = 0; i < lst.Count; i++)
                {
                    if (lst[i].Name == tag)
                    {
                        result.Add(lst[i]);

                        if (result.Count == limit.Value)
                            return result;
                    }

                    var childcollection = lst[i].ChildNodes;
                    if (childcollection == null) continue;
                    lst.AddRange(childcollection);
                }

                return result;
            }
            case false when emptyTag == false && limit != null:
            {
                lst.Add(doc);
                for (var i = 0; i < lst.Count; i++)
                {
                    if (lst[i].GetClasses().Contains(className) && lst[i].Name == tag)
                    {
                        result.Add(lst[i]);

                        if (result.Count == limit.Value)
                            return result;
                    }

                    var childcollection = lst[i].ChildNodes;
                    if (childcollection == null) continue;
                    lst.AddRange(childcollection);
                }

                return result;
            }
            default:
                throw new ArgumentException();
        }
    }
}