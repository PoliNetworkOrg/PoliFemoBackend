#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Article.News;
using PoliFemoBackend.Source.Objects.Threading;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class PoliMiNewsUtil
{
    internal const string UrlPoliMiNews = "https://www.polimi.it/in-evidenza";
    internal const string UrlPoliMiHomePage = "https://www.polimi.it/";
    private const int PoliMiAuthorId = 1;


    internal static IEnumerable<HtmlNews> Merge(HtmlNodeCollection? urls, IReadOnlyCollection<HtmlNode>? newsPolimi)
    {
        var result = new List<HtmlNews>();
        switch (urls)
        {
            case null when newsPolimi == null:
                return result;
            case null:
            {
                result.AddRange(newsPolimi.Select(item => new HtmlNews { NodePoliMiHomePage = item }));
                return result;
            }
            case not null when newsPolimi == null:
            {
                result.AddRange(urls.Select(item => new HtmlNews { NodeInEvidenza = item }));
                return result;
            }
        }

        var nodiPoliMiHomePage = newsPolimi.Select(item => new NodeFlagged { HtmlNode = item }).ToList();
        var nodiInEvidenza = urls.Select(item => new NodeFlagged { HtmlNode = item }).ToList();

        return MergeNotNull(nodiPoliMiHomePage, nodiInEvidenza);
    }

    private static IEnumerable<HtmlNews> MergeNotNull(IReadOnlyList<NodeFlagged> nodiPoliMiHomePage,
        IReadOnlyList<NodeFlagged> nodiInEvidenza)
    {
        var result = new List<HtmlNews>();
        foreach (var itemHomePage in nodiPoliMiHomePage)
        {
            if (itemHomePage.Flagged)
                continue;

            foreach (var itemInEvidenza in nodiInEvidenza)
            {
                var equals = TestIfEqual(itemHomePage, itemInEvidenza);
                if (!equals)
                    continue;

                itemHomePage.Flagged = true;
                itemInEvidenza.Flagged = true;
                result.Add(new HtmlNews
                    { NodeInEvidenza = itemInEvidenza.HtmlNode, NodePoliMiHomePage = itemHomePage.HtmlNode });
                break;
            }
        }

        result.AddRange(from t in nodiPoliMiHomePage
            where t.Flagged == false
            select new HtmlNews { NodePoliMiHomePage = t.HtmlNode });
        result.AddRange(from t in nodiInEvidenza
            where t.Flagged == false
            select new HtmlNews { NodeInEvidenza = t.HtmlNode });

        return result;
    }

    private static bool TestIfEqual(NodeFlagged itemHomePage, NodeFlagged itemInEvidenza)
    {
        if (itemHomePage.HtmlNode == null || itemInEvidenza.HtmlNode == null)
            return false;

        var hrefHomePage = HtmlUtil.GetElementsByTagAndClassName(itemHomePage.HtmlNode, "a")?.First().Attributes;
        var hrefInEvidenza = HtmlUtil.GetElementsByTagAndClassName(itemInEvidenza.HtmlNode, "a")?.First().Attributes;

        if (hrefHomePage == null || hrefInEvidenza == null)
            return false;

        var isPresentHrefInHomePage = hrefHomePage.Contains("href");
        var isPresentHrefInEvidenza = hrefInEvidenza.Contains("href");

        if (!isPresentHrefInEvidenza || !isPresentHrefInHomePage)
            return false;

        var hInHomePage = hrefHomePage["href"].Value;
        var hInEvidenza = hrefInEvidenza["href"].Value;

        if (string.IsNullOrEmpty(hInHomePage) || string.IsNullOrEmpty(hInEvidenza))
            return false;

        return CheckIfSimilar(hInEvidenza, hInHomePage);
    }

    private static bool CheckIfSimilar(string a, string b)
    {
        if (a == b)
            return true;

        var aS = a.Split("/");
        var bS = b.Split("/");

        var aS2 = aS.Where(x => !string.IsNullOrEmpty(x)).ToList();
        var bS2 = bS.Where(x => !string.IsNullOrEmpty(x)).ToList();

        if (aS2.Count != bS2.Count)
            return false;

        var matches = CountMatches(aS2, bS2);
        return matches >= aS2.Count / 2 && aS2[^1] == bS2[^1];
    }

    private static int CountMatches(IReadOnlyCollection<string> aS2, IReadOnlyList<string> bS2)
    {
        return aS2.Count != bS2.Count ? 0 : aS2.Where((t, i) => t == bS2[i]).Count();
    }


    internal static List<HtmlNode>? GetNewsPoliMi(HtmlDocument? docPoliMi)
    {
        var slider = HtmlUtil.GetElementsByTagAndClassName(docPoliMi?.DocumentNode, "body", null);
        var slider2 = HtmlUtil.GetElementsByTagAndClassName(slider?.First(), "section");
        var slider3 = slider2?.First(x => x.Id == "news");
        var slider4 = HtmlUtil.GetElementsByTagAndClassName(slider3, "div");
        var slider5 = slider4?.Where(x => x.GetClasses().Contains("sp-slide")).ToList();
        return slider5;
    }

    internal static HtmlDocument? LoadUrl(string url)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);
        return doc;
    }


    internal static void GetContent(NewsPolimi? result)
    {
        var web = new HtmlWeb();
        var doc = web.Load(result?.GetUrl());
        var urls1 = doc.DocumentNode.SelectNodes("//div");
        try
        {
            var urls = urls1.First(x => x.GetClasses().Contains("news-single-item"));
            var p = HtmlUtil.GetElementsByTagAndClassName(urls, "p")?.Select(x => x.InnerHtml).ToList();
            if (p != null)
                result?.SetContent(p);
        }
        catch
        {
            // ignored
        }

        if (!(result?.IsContentEmpty() ?? false))
            return;

        var urls2 = urls1.Where(x => x.GetClasses().Contains("container")).ToList();
        SetContent(urls2, result);
    }

    private static void SetContent(IReadOnlyCollection<HtmlNode> urls2, NewsPolimi newsPolimi)
    {
        var urls3 = HtmlUtil.GetElementsByTagAndClassName(urls2, "img");
        AdaptImages(urls3);

        newsPolimi.SetContent(urls2.Select(x => x.OuterHtml).ToList());
    }

    private static void AdaptImages(IEnumerable<HtmlNode>? urls3)
    {
        if (urls3 == null) return;

        foreach (var x in urls3) AdaptImage(x);
    }


    private static void AdaptImage(HtmlNode htmlNode)
    {
        var src = htmlNode.Attributes.Contains("src") ? htmlNode.Attributes["src"].Value : "";

        if (!src.StartsWith("http"))
            src = "https://polimi.it" + src;

        htmlNode.SetAttributeValue("src", src);
    }

    /// <summary>
    ///     Loops every 30 mins to sync PoliMi news with the app db
    /// </summary>
    /// <param name="threadWithAction">The running thread</param>
    public static void LoopGetNews(ThreadWithAction threadWithAction)
    {
        const int timeToWait = 1000 * 60 * 30; //30 mins
        var count = 0;
        while (true)
        {
            try
            {
                var r = GetNews();
                count += r;

                threadWithAction.Partial = r;
                threadWithAction.Total = count;
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
    ///     Get the latest news from PoliMi and stores them in the database
    /// </summary>
    private static int GetNews()
    {
        var news = DownloadNewsUtil.DownloadCurrentNews();
        var count = 0;
        foreach (var newsItem in news)
            try
            {
                var r = UpdateDbWithNews(newsItem);
                if (r == DoneEnum.DONE)
                    count++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        return count;
    }

    private static DoneEnum UpdateDbWithNews(NewsPolimi newsItem)
    {
        var url = newsItem.GetUrl();
        if (string.IsNullOrEmpty(url))
            return DoneEnum.ERROR;

        const string query = "SELECT COUNT(*) FROM Articles WHERE sourceUrl = @url";
        var args = new Dictionary<string, object?> { { "@url", url } };
        var results = Database.Database.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return DoneEnum.SKIPPED;

        var result = Database.Database.GetFirstValueFromDataTable(results);
        if (result == null)
            return DoneEnum.SKIPPED;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return DoneEnum.SKIPPED; //news already in db

        InsertItemInDb(newsItem);
        return DoneEnum.DONE;
    }

    private static void InsertItemInDb(NewsPolimi newsItem) //11111
    {
        const string query1 = "INSERT IGNORE INTO Articles " +
                              "(title,subtitle,content,publishTime,sourceUrl,id_author,image,id_tag) " +
                              "VALUES " +
                              "(@title,@subtitle,@text_,@publishTime,@sourceUrl, @author_id, @image, @tag)";
        var args1 = new Dictionary<string, object?>
        {
            { "@sourceUrl", newsItem.GetUrl() },
            { "@title", newsItem.GetTitle() },
            { "@subtitle", newsItem.GetSubtitle() },
            { "@text_", newsItem.GetContentAsTextJson() },
            { "@publishTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@author_id", PoliMiAuthorId },
            { "@image", newsItem.GetImgUrl() },
            { "@tag", newsItem.GetTag()?.ToUpper() == "" ? "ALTRO" : newsItem.GetTag()?.ToUpper() }
        };
        Database.Database.Execute(query1, GlobalVariables.GetDbConfig(), args1);
    }

    public static void FixContent(NewsPolimi result)
    {
        result.FixContent();
    }
}