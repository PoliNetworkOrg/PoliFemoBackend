using HtmlAgilityPack;

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

    internal static HtmlDocument? LoadUrl(string url)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);
        return doc;
    }
}