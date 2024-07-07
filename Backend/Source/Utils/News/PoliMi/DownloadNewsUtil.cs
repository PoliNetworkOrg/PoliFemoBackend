#region

using PoliFemoBackend.Source.Objects.Articles.News;
using PoliNetwork.Html.Utils;
using CoreGlobals = PoliNetwork.Core.Data.GlobalVariables;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class DownloadNewsUtil
{
    internal static IEnumerable<ArticleNews> DownloadCurrentNews()
    {
        try
        {
            // Get news from the Polimi news page
            var docNews = HtmlPageUtil.LoadUrl(PoliMiNewsUtil.UrlPoliMiNews);
            var newsCards =
                NodeUtil.GetElementsByTagAndClassName(
                    docNews?.DocumentNode,
                    "div",
                    "card--editorial-photo"
                ) ?? new List<HtmlAgilityPack.HtmlNode>();

            // Filter & parse the news
            var newsobjlist = newsCards?.Select(ExtractNews).ToList();

            return (from item in newsobjlist where item != null select item).ToList();
        }
        catch (InvalidOperationException ex)
        {
            CoreGlobals.DefaultLogger.Error(
                "There was an error parsing the polimi page, search skipped: " + ex
            );
            return new List<ArticleNews>();
        }
    }

    private static ArticleNews? ExtractNews(HtmlAgilityPack.HtmlNode htmlNews)
    {
        try
        {
            string url =
                "https://polimi.it"
                    + htmlNews?.SelectSingleNode(".//a")?.GetAttributeValue("href", "")
                ?? "";

            string img =
                NodeUtil
                    .GetElementsByTagAndClassName(htmlNews, "img")
                    ?.First()
                    ?.GetAttributeValue("src", "") ?? "";
            img = img.StartsWith("http") ? img : "https://polimi.it" + img;

            string tag = "tags_dalpoli";
            var result = new ArticleNews(tag, img);

            var cts = ArticleContent.LoadContentFromURL(url ?? "");
            result.AddContent(cts[0]);
            result.AddContent(cts[1]);

            return result;
        }
        catch (Exception ex)
        {
            CoreGlobals.DefaultLogger.Error(
                "There was an error parsing the news page, skipping: " + ex
            );
            return null;
        }
    }
}
