#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils.Html;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class RoomUtil
{
    public const string RoomInfoUrls = "https://www7.ceda.polimi.it/spazi/spazi/controller/";


    internal static async Task<List<HtmlNode>?> GetDailySituationOnDate(DateTime? date, string sede)
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

        var html = await HtmlUtil.DownloadHtmlAsync(url, false, true);
        if (html.IsValid() == false) return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(html.GetData());
        List<HtmlNode> nodes = new();

        var node = new HtmlNode(HtmlNodeType.Element, doc, 0)
        {
            InnerHtml = doc.DocumentNode.InnerHtml
        };
        nodes.Add(node);
        return nodes;
    }
}