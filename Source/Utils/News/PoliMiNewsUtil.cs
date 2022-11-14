#region

using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects;
using PoliFemoBackend.Source.Objects.Article;

#endregion

namespace PoliFemoBackend.Source.Utils.News;

public static class PoliMiNewsUtil
{
    public static List<JObject> DownloadCurrentNewsAsArticles()
    {
        var list = DownloadCurrentNews();
        return list.Select(x => x.ToArticle()).ToList();
    }

    private static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
        const string url = "https://www.polimi.it/in-evidenza";
        var web = new HtmlWeb();
        var doc = web.Load(url);
        var urls = doc.DocumentNode.SelectNodes("//ul").First(x => x.GetClasses().Contains("ce-menu"));
        return urls.ChildNodes.Select(ExtractNews).ToList();
    }

    private static NewsPolimi ExtractNews(HtmlNode htmlNode)
    {
     
        var selectMany = htmlNode.ChildNodes.SelectMany(x => x.ChildNodes);
        var htmlNodes = selectMany.Where(x => x.Attributes.Contains("href"));
        var enumerable = htmlNodes.Select(x => x.Attributes["href"].Value);
        var where = enumerable.Where(x => !string.IsNullOrEmpty(x));
        var url1 = where.FirstOrDefault("");
        
        var internalNews = !(url1.StartsWith("https://") || url1.StartsWith("http://"));
        var url2 = !internalNews ? url1 : "https://www.polimi.it/" + url1;
        var title = htmlNode.ChildNodes[0].InnerText.Trim();
        var subtitle = htmlNode.ChildNodes[1].ChildNodes[0].InnerText.Trim();
        
        var result = new NewsPolimi(internalNews, url2, title, subtitle);
        
        if (internalNews) 
            GetContent(result);

        return result;
    }

    private static void GetContent(NewsPolimi result)
    {
        var web = new HtmlWeb();
        var doc = web.Load(result.GetUrl());
        var urls1 = doc.DocumentNode.SelectNodes("//div");
        try
        {
            var urls = urls1.First(x => x.GetClasses().Contains("news-single-item"));
            var p = HtmlUtil.GetElementsByTagAndClassName(urls, "p")?.Select(x => x.InnerHtml).ToList();
            if (p != null)
                result.SetContent(p);
        }
        catch
        {
            // ignored
        }
    }

    public static void LoopGetNews()
    {
        throw new NotImplementedException();
    }
}