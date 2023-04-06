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


    internal static List<HtmlNode>? GetNewsPoliMi(HtmlDocument? docPoliMi)
    {
        var slider = NodeUtil.GetElementsByTagAndClassName(docPoliMi?.DocumentNode, "body", null);
        var slider2 = NodeUtil.GetElementsByTagAndClassName(slider?.First(), "section");
        var slider3 = slider2?.First(x => x.Id == "news");
        var slider4 = NodeUtil.GetElementsByTagAndClassName(slider3, "div");
        var slider5 = slider4?.Where(x => x.GetClasses().Contains("sp-slide")).ToList();
        return slider5;
    }

    internal static void TryAdjustContent(NewsPolimi? result)
    {
        var web = new HtmlWeb();
        var doc = web.Load(result?.GetUrl());
        var urls1 = doc.DocumentNode.SelectNodes("//div");
        try
        {
            TrySetContent1(result, urls1);
        }
        catch
        {
            // ignored
        }

        if (!(result?.IsContentEmpty() ?? false))
            return;

        try
        {
            TrySetContent2(result, urls1);
        }
        catch
        {
            // ignored
        }
    }

    private static void TrySetContent2(NewsPolimi result, HtmlNodeCollection urls1)
    {
        var urls2 = urls1.Where(x =>
            x.GetClasses().Contains("container") && !x.GetClasses().Contains("frame-type-header")
        ).ToList();
        urls2 = FlatHtml.FlatMap(urls2);
        HtmlNewsUtil.SetContent(urls2, result);
    }



    public static bool Predicate(ArticlePiece? x)
    {
        return x != null && !x.IsEmpty();
    }
    
    private static void TrySetContent1(NewsPolimi? result, HtmlNodeCollection urls1)
    {
        var urls = urls1.First(x => x.GetClasses().Contains("news-single-item"));

        var elementsByTagAndClassName = NodeUtil.GetElementsByTagAndClassName(urls, "p");
        elementsByTagAndClassName = FlatHtml.FlatMap(elementsByTagAndClassName);


        var selector = (Func<HtmlNode, ArticlePiece?>)ArticlePiece.Selector;
        var predicate = (Func<ArticlePiece?, bool>)Predicate;
        var articlePieces = elementsByTagAndClassName.Select(selector);
        var enumerable = articlePieces.Where(predicate);
        var p = enumerable.ToList();
        result?.SetContent(p);
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