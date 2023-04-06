using HtmlAgilityPack;

namespace PoliFemoBackend.Source.Utils.Html;

public class FlatHtml
{
    internal static List<HtmlNode> FlatMap(List<HtmlNode>? list)
    {
        if (list == null)
            return new List<HtmlNode>();

        while (true)
        {
            ;
            if (list.All(x => x.ChildNodes.Count == 0))
                return list;

            var list2 = new List<HtmlNode>();
            foreach (var v1 in list)
            {
                if (v1.ChildNodes.Count== 0)
                    list2.Add(v1);
                else
                {
                    list2.AddRange(v1.ChildNodes);
                }
            }

            list = list2;
        }
    }
}