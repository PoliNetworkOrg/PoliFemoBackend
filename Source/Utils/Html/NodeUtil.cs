using HtmlAgilityPack;

namespace PoliFemoBackend.Source.Utils.Html;

public class NodeUtil
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

                    if (result.Count == limit.Value) return result;
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

                        if (result.Count == limit.Value) return result;
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

                        if (result.Count == limit.Value) return result;
                    }

                    var childcollection = lst[i].ChildNodes;
                    if (childcollection == null) continue;

                    lst.AddRange(childcollection);
                }

                return result;
            }
            default:
                return null;
        }
    }

    public static IEnumerable<HtmlNode> GetElementsByTagAndClassName(IEnumerable<HtmlNode> list, string tag)
    {
        var results = new List<HtmlNode>();
        foreach (var r in list.Select(x => GetElementsByTagAndClassName(x, tag)))
            if (r != null)
                results.AddRange(r);

        return results;
    }
}