#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils.Html;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class RoomUtil
{
    public const string RoomInfoUrls = "https://www7.ceda.polimi.it/spazi/spazi/controller/";


    internal static List<HtmlNode>? GetDailySituationOnDate(DateTime? date, string sede)
    {
        date ??= DateTime.Today;
        var day = date?.Day;
        var month = date?.Month;
        var year = date?.Year;

        if (string.IsNullOrEmpty(sede)) return null;

        var url = "https://www7.ceda.polimi.it/spazi/spazi/controller/OccupazioniGiornoEsatto.do?" +
                  "csic=" + sede +
                  "&categoria=tutte" +
                  "&tipologia=tutte" +
                  "&giorno_day=" + day +
                  "&giorno_month=" + month +
                  "&giorno_year=" + year +
                  "&jaf_giorno_date_format=dd%2FMM%2Fyyyy&evn_visualizza=";

        var expireDate = DateTime.Now.AddHours(1);
        var html = HtmlUtil.DownloadHtmlAsync(url, expireDate);
        if (html.IsValid() == false) return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(html.GetData());

        var t1 = NodeUtil.GetElementsByTagAndClassName(doc.DocumentNode, "", "BoxInfoCard", 1);

        //Get html node tbody (table) containing the rooms' daily situation requested by the query 
        var t3 = NodeUtil.GetElementsByTagAndClassName(t1?[0], "", "scrollContent");
        return t3;
    }
}