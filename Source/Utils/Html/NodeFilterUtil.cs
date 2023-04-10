using System.Diagnostics.CodeAnalysis;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Html;

public static class NodeFilterUtil
{
    internal static List<HtmlNodeExtended?>? HasClassnameAndTagAndLimit(HtmlNodeExtended doc, List<string>? tag,
        string? className,
        [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?>? result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ?? false;
            var htmlNodeName = lst[i]?.HtmlNode?.Name;
            if (htmlNodeName != null)
            {
                var contains1 = tag?.Contains(htmlNodeName) ?? false;
                if (contains && contains1)
                {
                    result?.Add(lst[i]);

                    if (result?.Count == limit.Value) return result;
                }
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection) lst.Add(HtmlNodeExtended.From(VARIABLE));
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasNoClassnameAndLimit(HtmlNodeExtended doc, List<string>? tag,
        [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var htmlNodeName = lst[i]?.HtmlNode?.Name;
            if (htmlNodeName != null)
            {
                var contains = tag?.Contains(htmlNodeName) ?? false;
                if (contains)
                {
                    result.Add(lst[i]);

                    if (result.Count == limit.Value) return result;
                }
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection.Select(variable => HtmlNodeExtended.From(variable)));
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasTagAndLimit(HtmlNodeExtended doc, string? className,
        [DisallowNull] long? limit,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ?? false;
            if (contains)
            {
                result.Add(lst[i]);

                if (result.Count == limit.Value) return result;
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            foreach (var VARIABLE in childcollection) lst.Add(HtmlNodeExtended.From(VARIABLE));
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? HasClassnameAndTagAndNoLimit(HtmlNodeExtended doc, List<string>? tag,
        string? className,
        List<HtmlNodeExtended?> lst, List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var contains = lst[i]?.HtmlNode?.GetClasses().Contains(className) ?? false;
            var htmlNodeName = lst[i]?.HtmlNode?.Name;
            if (htmlNodeName != null)
            {
                var contains1 = tag?.Contains(htmlNodeName) ?? false;
                if (contains && contains1) result.Add(lst[i]);
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection.Select(HtmlNodeExtended.From));
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? EmptyClassnameAndNoLimit(HtmlNodeExtended doc, List<string>? tag,
        List<HtmlNodeExtended?> lst,
        List<HtmlNodeExtended?> result)
    {
        lst.Add(doc);
        for (var i = 0; i < lst.Count; i++)
        {
            var htmlNodeName = lst[i]?.HtmlNode?.Name;
            if (htmlNodeName != null)
            {
                var contains = tag?.Contains(htmlNodeName) ?? false;
                if (contains)
                    result.Add(lst[i]);
            }

            var childcollection = lst[i]?.HtmlNode?.ChildNodes;
            if (childcollection == null) continue;

            lst.AddRange(childcollection.Select(HtmlNodeExtended.From));
        }

        return result;
    }

    internal static List<HtmlNodeExtended?>? EmptyTagAndNoLimit(HtmlNodeExtended doc, string? className,
        List<HtmlNodeExtended?> lst,
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