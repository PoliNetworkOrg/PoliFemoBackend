using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.Html;

public static class FlatHtml
{
    /// <summary>
    ///     Prendi una lista di nodi e restituisci una lista piatta (di nodi senza figli)
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    internal static List<HtmlNodeExtended?> FlatMap(List<HtmlNodeExtended?>? list)
    {
        if (list == null)
            return new List<HtmlNodeExtended?>();

        while (true)
        {
            if (list.All(x => x?.HtmlNode == null || x.HtmlNode?.ChildNodes.Count == 0))
                return list;

            list = FlatStep(list) //do a step of converting child list with childrent to a flat one
                .SelectMany(x => x ?? new List<HtmlNodeExtended>())
                .ToList();
        }
    }

    private static IEnumerable<IEnumerable<HtmlNodeExtended?>?> FlatStep(IEnumerable<HtmlNodeExtended?> list)
    {
        var htmlNodeses = list.Select(v1 =>
        {
            HtmlNodeExtended? Selector(HtmlNodeExtended? variable)
            {
                return AddChild(variable, v1);
            }

            var selector = (Func<HtmlNodeExtended, HtmlNodeExtended?>)Selector;
            if (v1 == null)
                return new List<HtmlNodeExtended?>();

            var b = v1.HtmlNode == null || v1.HtmlNode?.ChildNodes.Count == 0;
            return b
                ? new List<HtmlNodeExtended> { v1 }
                : v1.HtmlNode?.ChildNodes.Select(x => selector(HtmlNodeExtended.From(x)));
        });
        return htmlNodeses;
    }

    private static HtmlNodeExtended? AddChild(HtmlNodeExtended? child, HtmlNodeExtended? parent)
    {
        if (parent == null)
            return null;

        var htmlNodeName = parent.HtmlNode?.Name;
        switch (htmlNodeName)
        {
            case "ul":
            {
                return child;
            }

            case "div":
            case "p":
            case "span":

            {
                return child;
            }

            case "a":
            {
                if (child?.HtmlNode != null)
                    child.HtmlNode.Name = htmlNodeName;
                var parentHtmlAttributeCollection =
                    parent.HtmlAttributeCollection ?? ToDict(parent.HtmlNode?.Attributes);
                if (child == null) return null;
                child.HtmlAttributeCollection ??= new Dictionary<string, string?>();
                foreach (var variable in parentHtmlAttributeCollection)
                    child.HtmlAttributeCollection[variable.Key] = variable.Value;

                return child;
            }
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
            case "em":

            case "sub":
            case "sup":
            case "blockquote":
            case "strong":
            case "figure":
            case "header":
            case "li":
            {
                ;
                if (child is { HtmlNode: not null }) child.HtmlNode.Name = htmlNodeName;
                return child;
            }

            default:
            {
                Console.WriteLine(htmlNodeName);
                break;
            }
        }

        return child;
    }

    public static Dictionary<string, string?> ToDict(HtmlAttributeCollection? htmlNodeAttributes)
    {
        if (htmlNodeAttributes == null)
            return new Dictionary<string, string?>();

        var r = new Dictionary<string, string?>();
        foreach (var variable in htmlNodeAttributes) r[variable.Name] = variable.Value;

        return r;
    }
}