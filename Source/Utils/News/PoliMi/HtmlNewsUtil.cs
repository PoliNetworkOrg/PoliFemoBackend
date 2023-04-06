using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class HtmlNewsUtil
{
    internal static bool CheckIfSimilar(string a, string b)
    {
        if (a == b)
            return true;

        var aS = a.Split("/");
        var bS = b.Split("/");

        var aS2 = aS.Where(x => !string.IsNullOrEmpty(x)).ToList();
        var bS2 = bS.Where(x => !string.IsNullOrEmpty(x)).ToList();

        if (aS2.Count != bS2.Count)
            return false;

        var matches = CountMatches(aS2, bS2);
        return matches >= aS2.Count / 2 && aS2[^1] == bS2[^1];
    }

    private static int CountMatches(IReadOnlyCollection<string> aS2, IReadOnlyList<string> bS2)
    {
        return aS2.Count != bS2.Count ? 0 : aS2.Where((t, i) => t == bS2[i]).Count();
    }

    internal static void SetContent(IReadOnlyCollection<HtmlNode> urls2, NewsPolimi newsPolimi)
    {
        var urls3 = NodeUtil.GetElementsByTagAndClassName(urls2, "img");
        AdaptImages(urls3);

        var selector = (Func<HtmlNode, ArticlePiece?>)ArticlePiece.Selector;
        var predicate = (Func<ArticlePiece?, bool>)ArticlePiece.Predicate;
        var articlePieces = urls2.Select(selector).Where(predicate).ToList();
        newsPolimi.SetContent(articlePieces);
    }

    private static void AdaptImages(IEnumerable<HtmlNode>? urls3)
    {
        if (urls3 == null) return;

        foreach (var x in urls3) AdaptImage(x);
    }


    private static void AdaptImage(HtmlNode htmlNode)
    {
        var src = htmlNode.Attributes.Contains("src") ? htmlNode.Attributes["src"].Value : "";

        if (!src.StartsWith("http"))
            src = "https://polimi.it" + src;

        htmlNode.SetAttributeValue("src", src);
    }

    internal static HtmlDocument? LoadUrl(string url)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);
        return doc;
    }
}