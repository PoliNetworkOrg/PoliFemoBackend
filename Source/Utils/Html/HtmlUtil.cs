#region

using System.Net;
using System.Text;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Web;

#endregion

namespace PoliFemoBackend.Source.Utils.Html;

public static class HtmlUtil
{
    internal static Task<WebReply> DownloadHtmlAsync(string urlAddress, bool isRoomTable = false)

    {
        try
        {
            var q = Database.Database.ExecuteSelect("SELECT * FROM WebCache WHERE url = @url", GlobalVariables.DbConfigVar, new Dictionary<string, object?> {{"@url", urlAddress}});
            if (q?.Rows.Count > 0)
            {
                var sq = q?.Rows[0]["content"]?.ToString();
                if (sq != null) return Task.FromResult(new WebReply(sq, HttpStatusCode.OK));
            }
            HttpClient httpClient = new();
            var task = httpClient.GetByteArrayAsync(urlAddress);
            task.Wait();
            var response = task.Result;
            var s = Encoding.UTF8.GetString(response, 0, response.Length);
            if (isRoomTable) {
                var doc = new HtmlDocument();
                doc.LoadHtml(s);
                var t1 = NodeUtil.GetElementsByTagAndClassName(doc.DocumentNode, "", "BoxInfoCard", 1);
                var t3 = NodeUtil.GetElementsByTagAndClassName(t1?[0], "", "scrollContent");
                s = t3?[0].InnerHtml ?? "";

            }
            Database.Database.Execute("INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW())", GlobalVariables.DbConfigVar, new Dictionary<string, object?> {{"@url", urlAddress}, {"@content", s}});
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