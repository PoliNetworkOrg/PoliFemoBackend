using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Html;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class DownloadNewsUtil
{
    internal static IEnumerable<ArticleNews> DownloadCurrentNews()
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

        return (from item in newsobjlist where item != null select item).ToList();
    }


    private static ArticleNews? ExtractNews(HtmlNews htmlNews)
    {
        if (htmlNews.NodeInEvidenza == null && htmlNews.NodePoliMiHomePage == null)
            return null;
        try
        {
            bool? internalNews = null;
            string? url2 = null;
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

                if (htmlNews.NodePoliMiHomePage != null)
                {
                    var img = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "img")?.First()
                        .Attributes["src"].Value ?? "";
                    tagFinal = NodeUtil.GetElementsByTagAndClassName(htmlNews.NodePoliMiHomePage, "span")
                        ?.First(x => x.GetClasses().Contains("newsCategory")).InnerHtml.Trim();
                    urlImgFinal = img.StartsWith("http") ? img : "https://polimi.it" + img;
                }
            }

            var result = new ArticleNews(tagFinal ?? "", urlImgFinal ?? "");
            if (internalNews ?? false) {
                var cts = ArticleContent.LoadContentFromURL(url2 ?? "");
                result.AddContent(cts[0]);
                result.AddContent(cts[1]);
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}