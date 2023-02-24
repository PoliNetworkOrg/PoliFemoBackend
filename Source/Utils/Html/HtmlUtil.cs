#region

using System.Net;
using System.Text;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Search;
using PoliFemoBackend.Source.Objects.Web;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class HtmlUtil
{
    private static readonly Dictionary<string, SearchResultTempObject> RecordsCache = new();


    public static WebReply DownloadHtmlAsync(string urlAddress, DateTime? expireDate = null,
        ExpireCacheEnum alreadyExpired = ExpireCacheEnum.ALREADY_EXPIRED)
    {
        if (string.IsNullOrEmpty(urlAddress))
            return new WebReply(null, HttpStatusCode.ExpectationFailed, false);

        //if expired we need to download it again
        switch (alreadyExpired)
        {
            case ExpireCacheEnum.NEVER_EXPIRE:
                break;
            case ExpireCacheEnum.ALREADY_EXPIRED:
                return DownloadNotFromCache(urlAddress, expireDate, alreadyExpired);
            case ExpireCacheEnum.TIMED_EXPIRATION:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(alreadyExpired), alreadyExpired, null);
        }

        //let's try to see if it's in the cache and not expired
        var possibleWebReply = TryToGetResultFromCache(urlAddress);
        return possibleWebReply ?? DownloadNotFromCache(urlAddress, expireDate, alreadyExpired);
    }

    private static WebReply? TryToGetResultFromCache(string urlAddress)
    {
        if (!RecordsCache.ContainsKey(urlAddress))
            return null;

        var record = RecordsCache[urlAddress];

        var expired = record.HasExpired();
        if (expired)
        {
            RecordsCache.Remove(urlAddress);
            return null;
        }

        //we have a match!
        var webReply = new WebReply(record.Result, HttpStatusCode.OK, true);
        return webReply;
    }

    private static WebReply DownloadNotFromCache(string urlAddress, DateTime? expireDate,
        ExpireCacheEnum alreadyExpired)
    {
        try
        {
            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;

            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            if (!string.IsNullOrEmpty(s))
                //if there's a value, cache it
                RecordsCache[urlAddress] = new SearchResultTempObject(s, expireDate, alreadyExpired);

            return new WebReply(s, HttpStatusCode.OK, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new WebReply(null, HttpStatusCode.ExpectationFailed, false);
    }
}