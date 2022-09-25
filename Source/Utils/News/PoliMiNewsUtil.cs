#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects;

#endregion

namespace PoliFemoBackend.Source.Utils.News;

public static class PoliMiNewsUtil
{
    public static List<NewsPolimi> DownloadCurrentNews()
    {
        const string url = "https://www.polimi.it/in-evidenza";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var urls = doc.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));
        return urls.ChildNodes.Select(ExtractNews).ToList();
    }

    private static NewsPolimi ExtractNews(HtmlNode htmlNode)
    {
        var result = new NewsPolimi();
        var selectMany = htmlNode.ChildNodes.SelectMany(x => x.ChildNodes);
        var htmlNodes = selectMany.Where(x => x.Attributes.Contains("href"));
        var enumerable = htmlNodes.Select(x => x.Attributes["href"].Value);
        var where = enumerable.Where(x => !string.IsNullOrEmpty(x));
        var url = where.FirstOrDefault("");
        result.internalNews = !(url.StartsWith("https://") || url.StartsWith("http://"));
        result.url = !result.internalNews ? url : "https://www.polimi.it/" + url;
        result.title = htmlNode.ChildNodes[0].InnerText.Trim();
        result.subtitle = htmlNode.ChildNodes[1].ChildNodes[0].InnerText.Trim();
        if (result.internalNews) GetContent(result);

        return result;
    }

    private static void GetContent(NewsPolimi result)
    {
        var web = new HtmlWeb();
        var doc = web.Load(result.url);
        var urls = doc.DocumentNode.SelectNodes("//div").First(x => x.GetClasses().Contains("news-single-item"));
        var p = HtmlUtil.GetElementsByTagAndClassName(urls, "p")?.Select(x => x.InnerHtml).ToList();
        result.content = p;
    }
}