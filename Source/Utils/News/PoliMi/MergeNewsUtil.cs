using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class MergeNewsUtil
{
    internal static IEnumerable<HtmlNews> Merge(HtmlNodeCollection? urls,
        IReadOnlyCollection<HtmlNodeExtended?>? newsPolimi)
    {
        var result = new List<HtmlNews>();
        switch (urls)
        {
            case null when newsPolimi == null:
                return result;
            case null:
            {
                result.AddRange(newsPolimi.Select(item => new HtmlNews { NodePoliMiHomePage = item?.HtmlNode }));
                return result;
            }
            case not null when newsPolimi == null:
            {
                result.AddRange(urls.Select(item => new HtmlNews { NodeInEvidenza = item }));
                return result;
            }
        }

        var nodiPoliMiHomePage = newsPolimi.Select(item => new NodeFlagged { HtmlNode = item?.HtmlNode }).ToList();
        var nodiInEvidenza = urls.Select(item => new NodeFlagged { HtmlNode = item }).ToList();

        return MergeNotNull(nodiPoliMiHomePage, nodiInEvidenza);
    }

    private static IEnumerable<HtmlNews> MergeNotNull(IReadOnlyList<NodeFlagged> nodiPoliMiHomePage,
        IReadOnlyList<NodeFlagged> nodiInEvidenza)
    {
        var result = new List<HtmlNews>();
        foreach (var itemHomePage in nodiPoliMiHomePage)
        {
            if (itemHomePage.Flagged)
                continue;

            foreach (var itemInEvidenza in nodiInEvidenza)
            {
                var equals = TestIfEqual(itemHomePage, itemInEvidenza);
                if (!equals)
                    continue;

                itemHomePage.Flagged = true;
                itemInEvidenza.Flagged = true;
                result.Add(new HtmlNews
                    { NodeInEvidenza = itemInEvidenza.HtmlNode, NodePoliMiHomePage = itemHomePage.HtmlNode });
                break;
            }
        }

        result.AddRange(from t in nodiPoliMiHomePage
            where t.Flagged == false
            select new HtmlNews { NodePoliMiHomePage = t.HtmlNode });
        result.AddRange(from t in nodiInEvidenza
            where t.Flagged == false
            select new HtmlNews { NodeInEvidenza = t.HtmlNode });

        return result;
    }

    private static bool TestIfEqual(NodeFlagged itemHomePage, NodeFlagged itemInEvidenza)
    {
        if (itemHomePage.HtmlNode == null || itemInEvidenza.HtmlNode == null)
            return false;

        var hrefHomePage = NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From(itemHomePage.HtmlNode), new List<string>(){"a"})
            ?.First()?.GetAttributes();
        var hrefInEvidenza = NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From(itemInEvidenza.HtmlNode), new List<string>(){"a"})
            ?.First()?.GetAttributes();

        if (hrefHomePage == null || hrefInEvidenza == null)
            return false;

        var isPresentHrefInHomePage = hrefHomePage.ContainsKey("href");
        var isPresentHrefInEvidenza = hrefInEvidenza.ContainsKey("href");

        if (!isPresentHrefInEvidenza || !isPresentHrefInHomePage)
            return false;

        var hInHomePage = hrefHomePage["href"];
        var hInEvidenza = hrefInEvidenza["href"];

        if (string.IsNullOrEmpty(hInHomePage) || string.IsNullOrEmpty(hInEvidenza))
            return false;

        return HtmlNewsUtil.CheckIfSimilar(hInEvidenza, hInHomePage);
    }
}