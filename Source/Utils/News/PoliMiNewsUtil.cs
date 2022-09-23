using System.Net;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Objects;

namespace PoliFemoBackend.Source.Utils.News;

public static class PoliMiNewsUtil
{
    public static List<Objects.NewsPolimi> DownloadCurrentNews()
    {
        const string url = "https://www.polimi.it/in-evidenza";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        //doc.DocumentNode.SelectNodes("//ul/li[@class='no']")
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
        result.url = url.StartsWith("https://") || url.StartsWith("http://") ? url : "https://www.polimi.it/" + url;
        ;
        result.title = htmlNode.ChildNodes[0].InnerText.Trim();
        result.desc = htmlNode.ChildNodes[1].ChildNodes[0].InnerText.Trim();
        return result;
    }
}