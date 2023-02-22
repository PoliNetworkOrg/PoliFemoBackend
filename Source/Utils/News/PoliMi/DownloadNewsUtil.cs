using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Objects.Types;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class DownloadNewsUtil
{
    internal static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
        var docNews = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiNews);
        var urls = docNews?.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));

        var docPoliMi = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiHomePage);
        var newsPolimi = PoliMiNewsUtil.GetNewsPoliMi(docPoliMi);
        var merged = PoliMiNewsUtil.Merge(urls?.ChildNodes, newsPolimi);

        return DownloadCurrentNews2(merged);
    }

    private static IEnumerable<NewsPolimi> DownloadCurrentNews2(IEnumerable<HtmlNews> merged)
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

            if (htmlNews.NodeInEvidenza == null)
            {
                var img = HtmlUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "img")?.First()
                    .Attributes["src"].Value ?? "";
                tagFinal = HtmlUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "span")
                    ?.First(x => x.GetClasses().Contains("newsCategory")).InnerHtml.Trim();
                urlImgFinal = img.StartsWith("http") ? img : "https://polimi.it" + img;
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
                    var img = HtmlUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "img")?.First()
                        .Attributes["src"].Value ?? "";
                    tagFinal = HtmlUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "span")
                        ?.First(x => x.GetClasses().Contains("newsCategory")).InnerHtml.Trim();
                    urlImgFinal = img.StartsWith("http") ? img : "https://polimi.it" + img;
                }
            }


            var result = new NewsPolimi(internalNews ?? false, url2 ?? "", title ?? "", subtitle ?? "", tagFinal ?? "",
                urlImgFinal ?? "");

            if (internalNews ?? false)
                PoliMiNewsUtil.GetContent(result);

            if (result.IsContentEmpty())
                PoliMiNewsUtil.GetContent(result);

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