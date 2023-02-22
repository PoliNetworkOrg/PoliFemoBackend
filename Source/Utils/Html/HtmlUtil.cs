#region

using System.Net;
using System.Text;
using PoliFemoBackend.Source.Objects.Web;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class HtmlUtil
{
    internal static Task<WebReply> DownloadHtmlAsync(string urlAddress)

    {
        try
        {
            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;
            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            return Task.FromResult(new WebReply(s, HttpStatusCode.OK));
            /*

            if (response.StatusCode != HttpStatusCode.OK) return new WebReply(null, response.StatusCode);

            var receiveStream = response.Content;
            try
            {
                var te = receiveStream.ReadAsByteArrayAsync().Result;
                var s = Encoding.UTF8.GetString(te, 0, te.Length);

                return new WebReply(s, HttpStatusCode.OK);
            }
            catch
            {
                return new WebReply(null, HttpStatusCode.ExpectationFailed);
            }
            */
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(new WebReply(null, HttpStatusCode.ExpectationFailed));
        }
    }
}