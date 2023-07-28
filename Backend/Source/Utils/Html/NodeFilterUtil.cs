#region

using System.Diagnostics.CodeAnalysis;
using HtmlAgilityPack;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class NodeFilterUtil
{
    internal static List<HtmlNode>? HasClassnameAndTagAndLimit(HtmlNode doc, string tag, string? className,
        [DisallowNull] long? limit,
        List<HtmlNode> lst, List<HtmlNode> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            if (lst[i].GetClasses().Contains(className) && lst[i].Name == tag)
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i].ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection);
        }

        return result;
    }

    internal static List<HtmlNode>? HasNoClassnameAndLimit(HtmlNode doc, string tag, [DisallowNull] long? limit,
        List<HtmlNode> lst, List<HtmlNode> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            if (lst[i].Name == tag)
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i].ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection);
        }

        return result;
    }

    internal static List<HtmlNode>? HasTagAndLimit(HtmlNode doc, string? className, [DisallowNull] long? limit,
        List<HtmlNode> lst, List<HtmlNode> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            if (lst[i].GetClasses().Contains(className))
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i].ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection);
        }

        return result;
    }

    internal static List<HtmlNode>? HasClassnameAndTagAndNoLimit(HtmlNode doc, string tag, string? className,
        List<HtmlNode> lst, List<HtmlNode> result)
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

    internal static List<HtmlNode>? EmptyClassnameAndNoLimit(HtmlNode doc, string tag, List<HtmlNode> lst,
        List<HtmlNode> result)
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

    internal static List<HtmlNode>? EmptyTagAndNoLimit(HtmlNode doc, string? className, List<HtmlNode> lst,
        List<HtmlNode> result)
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
}