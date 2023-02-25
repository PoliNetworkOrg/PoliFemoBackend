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
    //Cache for the HtmlUtil
    private static readonly Dictionary<string, SearchResultTempObject> RecordsCache = new();


    /// <summary>
    ///     Download an url and get a WebReply. Can be get a result from cache.
    /// </summary>
    /// <param name="urlAddress">Url address to get</param>
    /// <param name="expireDate">
    ///     If it's downloaded from the web, if expireDate is not null, this will be the Date when the
    ///     result will be marked as expired in cache
    /// </param>
    /// <param name="alreadyExpired">
    ///     If you don't want to use caching, you can ignore the current cache, delete it and get a
    ///     new value
    /// </param>
    /// <returns>The webreply from the web or the cache, according to parameters</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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

    /// <summary>
    ///     Try to get result from cache
    /// </summary>
    /// <param name="urlAddress">The url address we should have a cache value result for</param>
    /// <returns>A cached value if found and not expired, null otherwise</returns>
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

    /// <summary>
    ///     Download the result from the web, ignoring the cache. Store it in the cache when found.
    /// </summary>
    /// <param name="urlAddress">Url address to get</param>
    /// <param name="expireDate">When will this value expire? (can be null, never)</param>
    /// <param name="alreadyExpired">If you don't want to cache the result, set this as "ALREADY_EXPIRED"</param>
    /// <returns>WebReply from the web, not the cache.</returns>
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