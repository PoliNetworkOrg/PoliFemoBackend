using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Objects.Types;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class DownloadNewsUtil
{
    internal static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
        // Get news from the Polimi news page
        var docNews = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiNews);
        var urls = docNews?.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));

        // Get news from the Polimi home page
        var docPoliMi = HtmlNewsUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiHomePage);
        var newsPolimi = PoliMiNewsUtil.GetNewsPoliMi(docPoliMi);

        // Merge the two lists
        var newslist = MergeNewsUtil.Merge(urls?.ChildNodes, newsPolimi);

        // Filter & parse the news
        var newsobjlist = newslist.Select(ExtractNews).ToList();

        return (from item in newsobjlist where item.IsPresent select item.GetValue()).ToList();
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
                var img = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "img")?.First()
                    .Attributes["src"].Value ?? "";
                tagFinal = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "span")
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
                    var img = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "img")?.First()
                        .Attributes["src"].Value ?? "";
                    tagFinal = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "span")
                        ?.First(x => x.GetClasses().Contains("newsCategory")).InnerHtml.Trim();
                    urlImgFinal = img.StartsWith("http") ? img : "https://polimi.it" + img;
                }
            }


            var result = new NewsPolimi(internalNews ?? false, url2 ?? "", title ?? "", subtitle ?? "", tagFinal ?? "",
                urlImgFinal ?? "");

            if (internalNews ?? false)
                result.SetContent();

            return new Optional<NewsPolimi>(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new Optional<NewsPolimi>();
    }
}