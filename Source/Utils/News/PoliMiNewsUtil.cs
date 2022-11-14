#region

using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Article;
using PoliFemoBackend.Source.Objects.Threading;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.News;

public static class PoliMiNewsUtil
{
    private const string UrlPolimi = "https://www.polimi.it/in-evidenza";
    private const string PolimiName = "PoliMi";
    
    public static List<JObject> DownloadCurrentNewsAsArticles()
    {
        var list = DownloadCurrentNews();
        return list.Select(x => x.ToArticle()).ToList();
    }

    private static IEnumerable<NewsPolimi> DownloadCurrentNews()
    {
 
        var web = new HtmlWeb();
        var doc = web.Load(UrlPolimi);
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
        var polimiAuthorId = GetPolimiAuthorIdAndCreateItIfNotFound();
        
        const int timeToWait = 1000 * 60 * 30; //30 mins
        while (true)
        {
            try
            {
                GetNews(polimiAuthorId);
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
    /// Gets polimi author id and create if it doesn't exist.
    /// </summary>
    /// <returns>PoliMi author id</returns>
    /// <exception cref="Exception">If it can't be found nor created</exception>
    private static int GetPolimiAuthorIdAndCreateItIfNotFound()
    {
        var dt = Database.Database.ExecuteSelect(
            "SELECT COUNT(*) FROM Authors WHERE name_ = @name", 
            DbConfig.GetDbConfigNew(), 
            new Dictionary<string, object?>
            {
                    {"@name", PolimiName}
                }
            );
        
        if (dt == null)
            throw new Exception("[01] Can't detect if PoliMi author is present in authors table.");

        var result = Database.Database.GetFirstValueFromDataTable(dt);
        if (result == null)
            throw new Exception("[02] Can't detect if PoliMi author is present in authors table.");

        var num = Convert.ToInt32(result);
        if (num > 0)
        {
            return GetPolimiAuthorIdBecauseWeKnowItExists();
        }

        Database.Database.Execute("INSERT INTO Authors (name,link) VALUES (@name,@link)", DbConfig.GetDbConfigNew(), new Dictionary<string, object?>
        {
            {"@name", PolimiName },
            {"@link", UrlPolimi}
        });

        return GetPolimiAuthorIdBecauseWeKnowItExists();
    }

    /// <summary>
    /// We know that polimi author exists so we retrieve its id
    /// </summary>
    /// <returns>Polimi author id</returns>
    /// <exception cref="Exception">If it can't be found nor created</exception>
    private static int GetPolimiAuthorIdBecauseWeKnowItExists()
    {
        var dt = Database.Database.ExecuteSelect(
            "SELECT id_author  FROM Authors WHERE name_ = @name", 
            DbConfig.GetDbConfigNew(), 
            new Dictionary<string, object?>
            {
                {"@name", PolimiName}
            }
        );
        
        if (dt == null)
            throw new Exception("[03] Can't detect if PoliMi author is present in authors table.");

        var result = Database.Database.GetFirstValueFromDataTable(dt);
        if (result == null)
            throw new Exception("[04] Can't detect if PoliMi author is present in authors table.");

        var id = Convert.ToInt32(result);
        return id;
    }

    /// <summary>
    /// Get the latest news from PoliMi and stores them in the database
    /// </summary>
    private static void GetNews(int idPolimiAuthor)
    {
        var news = DownloadCurrentNews();
        foreach (var newsItem in news)
        {
            try
            {
                UpdateDbWithNews(newsItem, idPolimiAuthor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    private static void UpdateDbWithNews(NewsPolimi newsItem, int idPolimiAuthor)
    {
        var url = newsItem.GetUrl();
        if (string.IsNullOrEmpty(url))
            return;
        
        const string query = "SELECT COUNT(*) FROM Articles WHERE sourceUrl = @url";
        var args = new Dictionary<string, object?> { {"@url", url}};
        var results = Database.Database.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return;

        var result = Database.Database.GetFirstValueFromDataTable(results);
        if (result == null)
            return;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return; //news already in db

        InsertItemInDb(newsItem, idPolimiAuthor);
    }

    private static void InsertItemInDb(NewsPolimi newsItem, int idPolimiAuthor)
    {
        const string query1 = "INSERT INTO Articles " +
                             "(title,subtitle,text_,publishTime,sourceUrl) " +
                             "VALUES " +
                             "(@title,@subtitle,@text_,@publishTime,@sourceUrl)";
        var args1 = new Dictionary<string, object?>
        {
            {"@sourceUrl", newsItem.GetUrl()},
            {"@title", newsItem.GetTitle()},
            {"@subtitle", newsItem.GetSubtitle()},
            {"@text_", newsItem.GetContentAsTextJson()},
            {"@publishTime", DateTime.Now}
        };
        Database.Database.Execute(query1, GlobalVariables.GetDbConfig(), args1);
        
        
        var url = newsItem.GetUrl();
        if (string.IsNullOrEmpty(url))
            return;
        
        const string query2 = "SELECT id_article FROM Articles WHERE sourceUrl = @url";
        var args2 = new Dictionary<string, object?> { {"@url", url}};
        var results = Database.Database.ExecuteSelect(query2, GlobalVariables.GetDbConfig(), args2);
        if (results == null)
            return;

        var result = Database.Database.GetFirstValueFromDataTable(results);
        if (result == null)
            return;

        var idArticle = Convert.ToInt32(result);

        const string query3 = "INSERT INTO scritto (id_article,id_author) VALUES (@id_article, @id_author)";
        var args3 = new Dictionary<string, object?>
        {
            {"@id_article", idArticle},
            {"@id_author", idPolimiAuthor}
        };
        Database.Database.Execute(query3, DbConfig.DbConfigVar, args3);

    }
}