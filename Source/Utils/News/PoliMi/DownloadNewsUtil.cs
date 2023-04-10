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

        var docPoliMi = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiHomePage);
        var newsPolimi = PoliMiNewsUtil.GetNewsPoliMi(docPoliMi);
        var merged = MergeNewsUtil.Merge(urls?.ChildNodes, newsPolimi).ToList();

        //Console.WriteLine(merged.Count);
        return ProcessDownloadedNews(merged);
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

            var elementsByTagAndClassName = NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From(htmlNews.NodePoliMiHomePage), "img");
            var dictionary = elementsByTagAndClassName?.First()?.GetAttributes();
            if (htmlNews.NodeInEvidenza == null)
            {
                var containsKey = dictionary?.ContainsKey("src") ?? false;
                var img = containsKey ? dictionary?["src"] : "";

                bool Predicate(HtmlNodeExtended? x) => x?.HtmlNode?.GetClasses().Contains("newsCategory") ?? false;
                tagFinal = NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From(htmlNews.NodePoliMiHomePage), "span")
                    ?.First(Predicate)?.HtmlNode?.InnerHtml.Trim();
                var startsWith = img?.StartsWith("http") ??false;
                urlImgFinal = startsWith ? img : "https://polimi.it" + img;
            }
            else
            {
                var selectMany = htmlNews.NodeInEvidenza?.ChildNodes.SelectMany(x => x.ChildNodes);
                var htmlNodes = selectMany?.Where(x => x.Attributes.Contains("href"));
                var enumerable = htmlNodes?.Select(x => x.Attributes["href"].Value);
                var where = enumerable?.Where(x => !string.IsNullOrEmpty(x));
                var url1 = where?.FirstOrDefault("") ?? "";

                internalNews = !(url1.StartsWith("https://") || url1.StartsWith("http://"));
                url2 = !(internalNews ?? false) ? url1 : "https://www.polimi.it" + url1;
                var child = htmlNews.NodeInEvidenza?.ChildNodes;
                title = child?[0].InnerText.Trim();
                var child2 = child?[1].ChildNodes;
                if (child2?.Count > 0)
                    subtitle = child2[0].InnerText.Trim();

                if (htmlNews.NodePoliMiHomePage != null)
                {
                    var attributes = dictionary;
                    var containsKey = (attributes?.ContainsKey("src") ?? false);
                    var img = containsKey ? attributes?["src"] : "";

                    bool Predicate(HtmlNodeExtended? x) => x?.HtmlNode?.GetClasses().Contains("newsCategory") ?? false;
                    tagFinal = NodeUtil.GetElementsByTagAndClassName(HtmlNodeExtended.From( htmlNews.NodePoliMiHomePage), "span")
                        ?.First(Predicate)?.HtmlNode?.InnerHtml.Trim();
                    var startsWith = img?.StartsWith("http") ?? false;
                    urlImgFinal = startsWith ? img : "https://polimi.it" + img;
                }
            }


            var result = new NewsPolimi(internalNews ?? false, url2 ?? "", title ?? "", subtitle ?? "", tagFinal ?? "",
                urlImgFinal ?? "");

            if (internalNews ?? false)
                PoliMiNewsUtil.TryAdjustContent(result);

            if (result.IsContentEmpty())
                PoliMiNewsUtil.TryAdjustContent(result);

            FixContent(result);

            return new Optional<NewsPolimi>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new Optional<NewsPolimi>();
    }


    private static void FixContent(NewsPolimi result)
    {
        result.FixContent();
    }
}