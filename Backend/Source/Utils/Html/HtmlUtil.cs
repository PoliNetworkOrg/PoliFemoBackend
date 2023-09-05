#region

using System.Net;
using System.Text;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Web;
using PoliFemoBackend.Source.Utils.Cache;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class HtmlUtil
{
    internal static Task<WebReply> DownloadHtmlAsync(
        string urlAddress,
        bool useCache = true,
        CacheTypeEnum cacheTypeEnum = CacheTypeEnum.NONE)
    {
        try
        {
            if (useCache)
            {
                var resultFromCache = CacheUtil.CheckIfToUseCache(urlAddress);
                if (resultFromCache != null)
                    return Task.FromResult(resultFromCache);
            }

            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;
            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            //s = FixTableContentFromCache(cacheTypeEnum, s);

            switch (cacheTypeEnum)
            {
                case CacheTypeEnum.ROOMTABLE:
                    s = FixFromTableRoomCache(s);
                    break;
                default:
                    break;
            }

            if (useCache)
                CacheUtil.SaveToCache(urlAddress, s);

            return Task.FromResult(new WebReply(s, HttpStatusCode.OK));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(new WebReply(null, HttpStatusCode.ExpectationFailed));
        }
    }

    private static string FixFromTableRoomCache(string s)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(s);
        var t1 = NodeUtil.GetElementsByTagAndClassName(doc.DocumentNode, "", "BoxInfoCard", 1);
        var t3 = NodeUtil.GetElementsByTagAndClassName(t1?[0], "", "scrollContent");
        s = t3?[0].InnerHtml ?? "";
        return s;
    }

}