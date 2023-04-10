using System.Diagnostics.CodeAnalysis;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Html;

public static class NodeFilterUtil
{
    internal static List<HtmlNodeExtended?>? HasClassnameAndTagAndLimit(HtmlNodeExtended doc, string tag, string? className,
        [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?>? result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ??false;
            if (contains && lst[i]?.HtmlNode?.Name == tag)
            {
                result?.Add(lst[i]);

                if (result?.Count == limit.Value) return result;
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection)
            {
                lst.Add(HtmlNodeExtended.From(VARIABLE));
            } 
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasNoClassnameAndLimit(HtmlNodeExtended doc, string tag, [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            if (lst[i]?.HtmlNode?.Name == tag)
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection)
            {
                lst.Add(HtmlNodeExtended.From(VARIABLE));
            } 
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasTagAndLimit(HtmlNodeExtended doc, string? className, [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ??false;
            if (contains)
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection)
            {
                lst.Add(HtmlNodeExtended.From(VARIABLE));
            } 
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasClassnameAndTagAndNoLimit(HtmlNodeExtended doc, string tag, string? className,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ?? false;
            if (contains && lst[i]?.HtmlNode?.Name == tag) result.Add(lst[i]);

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection)
            {
                lst.Add(HtmlNodeExtended.From(VARIABLE));
            } 
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? EmptyClassnameAndNoLimit(HtmlNodeExtended doc, string tag, List<HtmlNodeExtended?> lst,
        List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            if (lst[i]?.HtmlNode?.Name == tag) result.Add(lst[i]);

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection)
            {
                lst.Add(HtmlNodeExtended.From(VARIABLE));
            } 
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? EmptyTagAndNoLimit(HtmlNodeExtended doc, string? className, List<HtmlNodeExtended?> lst,
        List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ?? false;
            if (contains) result.Add(lst[i]);

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection.Select(HtmlNodeExtended.From));
        }

        return result;
    }
}