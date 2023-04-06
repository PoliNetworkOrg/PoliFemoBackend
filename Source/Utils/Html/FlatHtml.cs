using HtmlAgilityPack;

namespace PoliFemoBackend.Source.Utils.Html;

public static class FlatHtml
{
    /// <summary>
    /// Prendi una lista di nodi e restituisci una lista piatta (di nodi senza figli)
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

            var list2 = new List<HtmlNode>();
            foreach (var v1 in list)
            {
                if (v1.ChildNodes.Count== 0)
                    list2.Add(v1);
                else
                {
                    list2.AddRange(v1.ChildNodes.Select(variable => AddChild(variable, v1)));
                }
            }

            list = list2;
        }
    }

    private static HtmlNode AddChild(HtmlNode child, HtmlNode parent)
    {
        ;
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