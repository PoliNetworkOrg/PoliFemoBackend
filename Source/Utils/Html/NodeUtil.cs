using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Html;

public static class NodeUtil
{
    internal static List<HtmlNodeExtended?>? GetElementsByTagAndClassName(HtmlNodeExtended? doc, string tag = "",
        string? className = "", long? limit = null)
    {
        if (doc == null) return null;

        var lst = new List<HtmlNodeExtended?>();
        var emptyTag = string.IsNullOrEmpty(tag);
        var emptyCn = string.IsNullOrEmpty(className);
        if (emptyTag && emptyCn) return null;

        if (limit is <= 0) return null;

        var result = new List<HtmlNodeExtended?>();

        if (emptyTag && limit == null) return NodeFilterUtil.EmptyTagAndNoLimit(doc, className, lst, result);

        if (emptyCn && limit == null) return NodeFilterUtil.EmptyClassnameAndNoLimit(doc, tag, lst, result);

        if (!emptyCn && emptyTag == false && limit == null)
            return NodeFilterUtil.HasClassnameAndTagAndNoLimit(doc, tag, className, lst, result);

        if (emptyTag && limit != null) return NodeFilterUtil.HasTagAndLimit(doc, className, limit, lst, result);

        if (emptyCn && limit != null) return NodeFilterUtil.HasNoClassnameAndLimit(doc, tag, limit, lst, result);

        if (!emptyCn && emptyTag == false && limit != null)
            return NodeFilterUtil.HasClassnameAndTagAndLimit(doc, tag, className, limit, lst, result);

        return null;
    }


    public static IEnumerable<HtmlNodeExtended?> GetElementsByTagAndClassName(IEnumerable<HtmlNodeExtended?> list, string tag)
    {
        var results = new List<HtmlNodeExtended?>();
        foreach (var r in list.Select(x => GetElementsByTagAndClassName(x, tag)))
            if (r != null)
                results.AddRange(r);

        return results;
    }
}