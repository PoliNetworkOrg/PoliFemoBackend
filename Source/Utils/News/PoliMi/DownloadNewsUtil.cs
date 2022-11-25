using PoliFemoBackend.Source.Objects.Article;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public class DownloadNewsUtil
{
    internal static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
        var docNews = News.PoliMi.PoliMiNewsUtil.LoadUrl(News.PoliMi.PoliMiNewsUtil.UrlPoliMiNews);
        var urls = docNews?.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));

        var docPoliMi = News.PoliMi.PoliMiNewsUtil.LoadUrl(News.PoliMi.PoliMiNewsUtil.UrlPoliMiHomePage);
        var newsPolimi = News.PoliMi.PoliMiNewsUtil.GetNewsPoliMi(docPoliMi);
        var merged = News.PoliMi.PoliMiNewsUtil.Merge(urls?.ChildNodes, newsPolimi);

        return DownloadCurrentNews2(merged);
    }

    private static IEnumerable<NewsPolimi> DownloadCurrentNews2(IEnumerable<HtmlNews> merged)
    {
        var merged2 = merged.Select(News.PoliMi.PoliMiNewsUtil.ExtractNews).ToList();
        return (from item in merged2 where item.IsPresent select item.GetValue()).ToList();
    }
}