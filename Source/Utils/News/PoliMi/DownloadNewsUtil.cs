using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Objects.Types;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class DownloadNewsUtil
{
    internal static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
        var docNews = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiNews);
        var urls = docNews?.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));

        var merged = HtmlNewsList(urls);

        var filtroTest = merged.Where(x =>
        {
            var argNodePoliMiHomePage = x.NodeInEvidenza ?? x.NodePoliMiHomePage;
            return argNodePoliMiHomePage?.InnerText.ToLower().Contains("pasqua") ?? false;
        }).ToList();

        //Console.WriteLine(merged.Count);
        return ProcessDownloadedNews(filtroTest);
    }

    private static IEnumerable<HtmlNews> HtmlNewsList(HtmlNode? urls)
    {
        var docPoliMi = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiHomePage);
        var newsPolimi = PoliMiNewsUtil.GetNewsPoliMi(docPoliMi);
        var merged = MergeNewsUtil.Merge(urls?.ChildNodes, newsPolimi).ToList();
        return merged;
    }

    private static IEnumerable<NewsPolimi> ProcessDownloadedNews(IEnumerable<HtmlNews> merged)
    {
        var merged2 = merged.Select(ExtractNews).ToList();
        return (from item in merged2 where item.IsPresent select item.GetValue()).ToList();
    }


    private static Optional<NewsPolimi> ExtractNews(HtmlNews htmlNews)
    {
        if (htmlNews.NodeInEvidenza == null && htmlNews.NodePoliMiHomePage == null)
            return new Optional<NewsPolimi>();

        try
        {
            bool? internalNews = null;
            string? url2 = null;
            string? title = null;
            string? subtitle = null;
            string? urlImgFinal = null;
            string? tagFinal = null;

            var elementsByTagAndClassName =
                NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From(htmlNews.NodePoliMiHomePage), new List<string>(){"img"});
            var dictionary = elementsByTagAndClassName?.First()?.GetAttributes();
            if (htmlNews.NodeInEvidenza == null)
            {
                tagFinal = TagFinal(htmlNews, dictionary, out urlImgFinal);
            }
            else
            {
                internalNews = InternalNews(htmlNews, dictionary, out url2, out title, ref subtitle, ref tagFinal, ref urlImgFinal);
            }
            
            return Optional(internalNews, url2, title, subtitle, tagFinal, urlImgFinal);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new Optional<NewsPolimi>();
    }

    private static Optional<NewsPolimi> Optional(bool? internalNews, string? url2, string? title, string? subtitle, string? tagFinal,
        string? urlImgFinal)
    {
        var result = new NewsPolimi(internalNews ?? false, url2 ?? "", title ?? "", subtitle ?? "", tagFinal ?? "",
            urlImgFinal ?? "");

        if (internalNews ?? false)
            PoliMiNewsUtil.TryAdjustContent(result);

        if (result.IsContentEmpty())
            PoliMiNewsUtil.TryAdjustContent(result);

        FixContent(result);

        return new Optional<NewsPolimi>(result);
    }

    private static bool? InternalNews(HtmlNews htmlNews, IReadOnlyDictionary<string, string?>? dictionary, out string url2, out string? title,
        ref string? subtitle, ref string? tagFinal, ref string? urlImgFinal)
    {
        var selectMany = htmlNews.NodeInEvidenza?.ChildNodes.SelectMany(x => x.ChildNodes);
        var htmlNodes = selectMany?.Where(x => x.Attributes.Contains("href"));
        var enumerable = htmlNodes?.Select(x => x.Attributes["href"].Value);
        var where = enumerable?.Where(x => !string.IsNullOrEmpty(x));
        var url1 = where?.FirstOrDefault("") ?? "";

        bool? internalNews = !(url1.StartsWith("https://") || url1.StartsWith("http://"));
        url2 = !(internalNews ?? false) ? url1 : "https://www.polimi.it" + url1;
        var child = htmlNews.NodeInEvidenza?.ChildNodes;
        title = child?[0].InnerText.Trim();
        var child2 = child?[1].ChildNodes;
        if (child2?.Count > 0)
            subtitle = child2[0].InnerText.Trim();

        if (htmlNews.NodePoliMiHomePage == null)
            return internalNews;
        
        var containsKey = dictionary?.ContainsKey("src") ?? false;
        var img = containsKey ? dictionary?["src"] : "";

        bool Predicate(HtmlNodeExtended? x)
        {
            return x?.HtmlNode?.GetClasses().Contains("newsCategory") ?? false;
        }

        tagFinal = NodeUtil
            .GetElementsByTagAndClassName(HtmlNodeExtended.From(htmlNews.NodePoliMiHomePage), new List<string>(){"span"})
            ?.First(Predicate)?.HtmlNode?.InnerHtml.Trim();
        var startsWith = img?.StartsWith("http") ?? false;
        urlImgFinal = startsWith ? img : "https://polimi.it" + img;
        

        return internalNews;
    }

    private static string? TagFinal(HtmlNews htmlNews, IReadOnlyDictionary<string, string?>? dictionary, out string? urlImgFinal)
    {
        var containsKey = dictionary?.ContainsKey("src") ?? false;
        var img = containsKey ? dictionary?["src"] : "";

        bool Predicate(HtmlNodeExtended? x)
        {
            return x?.HtmlNode?.GetClasses().Contains("newsCategory") ?? false;
        }

        var tagFinal = NodeUtil
            .GetElementsByTagAndClassName(HtmlNodeExtended.From(htmlNews.NodePoliMiHomePage), new List<string>(){"span"})
            ?.First(Predicate)?.HtmlNode?.InnerHtml.Trim();
        var startsWith = img?.StartsWith("http") ?? false;
        urlImgFinal = startsWith ? img : "https://polimi.it" + img;
        return tagFinal;
    }


    private static void FixContent(NewsPolimi result)
    {
        result.FixContent();
    }
}