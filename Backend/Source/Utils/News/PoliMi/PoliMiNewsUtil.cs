#region

using PoliNetwork.Core.Enums;
using PoliNetwork.Core.Objects.Threading;
using CoreGlobals = PoliNetwork.Core.Data.GlobalVariables;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class PoliMiNewsUtil
{
    internal const string UrlPoliMiNews = "https://polimi.it/il-politecnico/news";

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
                CoreGlobals.DefaultLogger.Error(ex.ToString());
            }

            CoreGlobals.DefaultLogger.Debug("Finished autosearch of news.");
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
                var r = NewsUtil.UpdateDb(newsItem);
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
