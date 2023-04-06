using HtmlAgilityPack;

namespace PoliFemoBackend.Source.Utils.Html;

public static class FlatHtml
{
    /// <summary>
    ///     Prendi una lista di nodi e restituisci una lista piatta (di nodi senza figli)
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    internal static List<HtmlNode> FlatMap(List<HtmlNode>? list)
    {
        if (list == null)
            return new List<HtmlNode>();

        while (true)
        {
            if (list.All(x => x.ChildNodes.Count == 0))
                return list;

            list = FlatStep(list) //do a step of converting child list with childrent to a flat one
                .SelectMany(x => x)
                .ToList();
        }
    }

    private static IEnumerable<IEnumerable<HtmlNode>> FlatStep(IEnumerable<HtmlNode> list)
    {
        var htmlNodeses = list.Select(v1 =>
        {
            HtmlNode Selector(HtmlNode variable)
            {
                return AddChild(variable, v1);
            }

            var selector = (Func<HtmlNode, HtmlNode>)Selector;
            return v1.ChildNodes.Count == 0
                ? new List<HtmlNode> { v1 }
                : v1.ChildNodes.Select(selector);
        });
        return htmlNodeses;
    }

    private static HtmlNode AddChild(HtmlNode child, HtmlNode parent)
    {
        switch (parent.Name)
        {
            case "div":
            case "p":
            case "span":
            case "ul":
            {
                return child;
            }

            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "h6":
            case "em":
            case "a":
            case "sub":
            case "sup":
            case "blockquote":
            case "strong":
            case "figure":
            case "header":
            case "li":
            {
                child.Name = parent.Name;
                return child;
            }

            default:
            {
                Console.WriteLine(parent.Name);
                break;
            }
        }

        return child;
    }
}