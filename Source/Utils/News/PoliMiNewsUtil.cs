﻿#region

using System.Data;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Article;
using PoliFemoBackend.Source.Objects.Threading;

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

    /// <summary>
    /// Loops every 30 mins to sync PoliMi news with the app db
    /// </summary>
    /// <param name="threadWithAction">The running thread</param>
    public static void LoopGetNews(ThreadWithAction threadWithAction)
    {
        const int timeToWait = 1000 * 60 * 30; //30 mins
        while (true)
        {
            try
            {
                GetNews();
            }
            catch (Exception ex)
            {
                threadWithAction.Failed++;
                Console.WriteLine(ex);
            }
            
            Thread.Sleep(timeToWait);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    /// <summary>
    /// Get the latest news from PoliMi and stores them in the database
    /// </summary>
    private static void GetNews()
    {
        var news = DownloadCurrentNews();
        foreach (var newsItem in news)
        {
            UpdateDbWithNews(newsItem);
        }
    }

    private static void UpdateDbWithNews(NewsPolimi newsItem)
    {
        var url = newsItem.GetUrl();
        if (string.IsNullOrEmpty(url))
            return;
        
        const string query = "SELECT COUNT(*) FROM Articles WHERE sourceUrl = @url";
        var args = new Dictionary<string, object?>() { {"@url", url}};
        var results = Database.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return;

        var result = Database.GetFirstValueFromDataTable(results);
        if (result == null)
            return;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return; //news already in db

        InsertItemInDb(newsItem);
    }

    private static void InsertItemInDb(NewsPolimi newsItem)
    {
        const string query = "INSERT INTO Articles " +
                             "(title,subtitle,text_,publishTime,sourceUrl) " +
                             "VALUES " +
                             "(@title,@subtitle,@text_,@publishTime,@sourceUrl)";
        var args = new Dictionary<string, object?>()
        {
            {"@sourceUrl", newsItem.GetUrl()},
            {"@title", newsItem.GetTitle()},
            {"@subtitle", newsItem.GetSubtitle()},
            {"@text_", newsItem.GetContentAsTextJson()},
            {"@publishTime", DateTime.Now}
        };
        Database.Execute(query, GlobalVariables.GetDbConfig(), args);
    }
}