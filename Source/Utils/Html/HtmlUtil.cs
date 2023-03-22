#region

using System.Net;
using System.Text;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Web;

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
                var resultFromCache = CheckIfToUseCache(urlAddress);
                if (resultFromCache != null)
                    return Task.FromResult(resultFromCache);
            }

            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;
            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            s = FixTableContentFromCache(cacheTypeEnum, s);
            
            if (useCache)
                Cache.SaveToCacheUtil.SaveToCache(urlAddress, s);
            
            return Task.FromResult(new WebReply(s, HttpStatusCode.OK));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(new WebReply(null, HttpStatusCode.ExpectationFailed));
        }
    }

    private static string FixTableContentFromCache(CacheTypeEnum cacheTypeEnum, string s)
    {
        return cacheTypeEnum switch
        {
            CacheTypeEnum.NONE => s,
            CacheTypeEnum.ROOMTABLE => FixFromTableRoomCache(s),
            _ => s
        };
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


    private static WebReply? CheckIfToUseCache(string urlAddress)
    {
        var sq = Cache.GetCacheUtil.GetCache(urlAddress);
        return sq != null ? new WebReply(sq, HttpStatusCode.OK) : null;
    }
}