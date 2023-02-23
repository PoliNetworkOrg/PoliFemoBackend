#region

using System.Net;
using System.Text;
using PoliFemoBackend.Source.Objects.Web;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class HtmlUtil
{
    private static readonly Dictionary<string, Tuple<string?, DateTime?>> RecordsCache = new();


    public static WebReply DownloadHtmlAsync(string urlAddress, DateTime? expireDate = null)
    {
        if (string.IsNullOrEmpty(urlAddress))
            return new WebReply(null, HttpStatusCode.ExpectationFailed, false);

        var possibleWebReply = TryToGetResultFromCache(urlAddress);
        return possibleWebReply ?? DownloadNotFromCache(urlAddress, expireDate);
    }

    private static WebReply? TryToGetResultFromCache(string urlAddress)
    {
        if (!RecordsCache.ContainsKey(urlAddress))
            return null;

        var record = RecordsCache[urlAddress];

        if (!string.IsNullOrEmpty(record.Item1))
        {
            WebReply webReply;
            if (record.Item2 == null) //no expiration
            {
                webReply = new WebReply(record.Item1, HttpStatusCode.OK, true);
                return webReply;
            }

            if (record.Item2 <= DateTime.Now)
                //expired
            {
                RecordsCache.Remove(urlAddress);
            }
            else
            {
                webReply = new WebReply(record.Item1, HttpStatusCode.OK, true);
                return webReply;
            }
        }
        else
        {
            RecordsCache.Remove(urlAddress);
        }

        return null;
    }

    private static WebReply DownloadNotFromCache(string urlAddress, DateTime? expireDate)
    {
        try
        {
            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;
            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            if (!string.IsNullOrEmpty(s)) RecordsCache[urlAddress] = new Tuple<string?, DateTime?>(s, expireDate);

            return new WebReply(s, HttpStatusCode.OK, false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return new WebReply(null, HttpStatusCode.ExpectationFailed, false);
    }
}