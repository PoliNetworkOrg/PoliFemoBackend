#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Objects.Threading;
using PoliFemoBackend.Source.Utils.Html;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class PoliMiNewsUtil
{
    internal const string UrlPoliMiNews = "https://www.polimi.it/in-evidenza";
    internal const string UrlPoliMiHomePage = "https://www.polimi.it/";


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

        return HtmlNewsUtil.CheckIfSimilar(hInEvidenza, hInHomePage);
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
        HtmlNewsUtil.SetContent(urls2, result);
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

                threadWithAction.Partial.Add(r);
                threadWithAction.Total = count;
            }
            catch (Exception ex)
            {
                threadWithAction.Failed++;
                Logger.WriteLine(ex, LogSeverityLevel.Error);
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
                var r = NewsDbUtil.UpdateDbWithNews(newsItem);
                if (r == DoneEnum.DONE)
                    count++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        return count;
    }
}