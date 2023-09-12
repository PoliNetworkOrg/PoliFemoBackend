#region

using HtmlAgilityPack;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class NodeUtil
{
    internal static List<HtmlNode>? GetElementsByTagAndClassName(HtmlNode? doc, string tag = "",
        string? className = "", long? limit = null)
    {
        if (doc == null) return null;

        var lst = new List<HtmlNode>();
        var emptyTag = string.IsNullOrEmpty(tag);
        var emptyCn = string.IsNullOrEmpty(className);
        if (emptyTag && emptyCn) return null;

        if (limit is <= 0) return null;

        var result = new List<HtmlNode>();

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
}