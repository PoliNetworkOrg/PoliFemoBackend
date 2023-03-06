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
    internal static Task<WebReply> DownloadHtmlAsync(string urlAddress, bool useCache = true, HtmlConfigEnum isRoomTable = HtmlConfigEnum.NONE)

    {
        try
        {
            if (useCache)
            {
                var rs = GetFromCache(urlAddress);
                if (!string.IsNullOrEmpty(rs))
                    return Task.FromResult(new WebReply(rs, HttpStatusCode.OK));;
            }
            
            var s = GetFromWeb(urlAddress, isRoomTable);

            if (useCache)
            {
                AddToCache(s, urlAddress);
            }

            return Task.FromResult(new WebReply(s, HttpStatusCode.OK));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Task.FromResult(new WebReply(null, HttpStatusCode.ExpectationFailed));
        }
    }

    private static string GetFromWeb(string urlAddress, HtmlConfigEnum isRoomTable)
    {
        HttpClient httpClient = new();
        var task = httpClient.GetByteArrayAsync(urlAddress);
        task.Wait();
        var response = task.Result;
        var s = Encoding.UTF8.GetString(response, 0, response.Length);
        s = AdjustHtml(s, isRoomTable);
        return s;
    }

    private static string? GetFromCache(string urlAddress)
    {
        const string selectFromWebcacheWhereUrlUrl = "SELECT * FROM WebCache WHERE url = @url AND NOW() < expires_at";
        var dictionary = new Dictionary<string, object?> { { "@url", urlAddress } };
        var q = Database.Database.ExecuteSelect(
            selectFromWebcacheWhereUrlUrl,
            GlobalVariables.DbConfigVar,
            dictionary);
        return !(q?.Rows.Count > 0) ? null : q.Rows[0]["content"]?.ToString();
    }

    private static void AddToCache(string s, string urlAddress)
    {
        const string insertIntoWebcacheUrlContentExpiresAtValuesUrlContentNow = "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW())";
        var dictionary = new Dictionary<string, object?> {{"@url", urlAddress}, {"@content", s}};
        Database.Database.Execute(
            insertIntoWebcacheUrlContentExpiresAtValuesUrlContentNow, 
            GlobalVariables.DbConfigVar, 
            dictionary);
    }

    private static string AdjustHtml(string s, HtmlConfigEnum isRoomTable)
    {
        return isRoomTable switch
        {
            HtmlConfigEnum.NONE => s,
            HtmlConfigEnum.ROOM_TABLE => AdjustHtmlRoomtTable(s),
            _ => s
        };
    }

    private static string AdjustHtmlRoomtTable(string s)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(s);
        var t1 = NodeUtil.GetElementsByTagAndClassName(doc.DocumentNode, "", "BoxInfoCard", 1);
        var t3 = NodeUtil.GetElementsByTagAndClassName(t1?[0], "", "scrollContent");
        s = t3?[0].InnerHtml ?? "";
        return s;
    }
}