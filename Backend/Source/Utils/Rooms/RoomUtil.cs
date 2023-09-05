#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Utils.Html;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class RoomUtil
{
    public const string RoomInfoUrls = "https://www7.ceda.polimi.it/spazi/spazi/controller/";


    internal static async Task<Tuple<List<HtmlNode>?, string>> GetDailySituationOnDate(DateTime? date, string sede)
    {
        date ??= DateTime.Today;
        var day = date?.Day;
        var month = date?.Month;
        var year = date?.Year;

        if (string.IsNullOrEmpty(sede)) return new Tuple<List<HtmlNode>?, string>(null, "sede empty");

        var url = "https://www7.ceda.polimi.it/spazi/spazi/controller/OccupazioniGiornoEsatto.do?" +
                  "csic=" + sede +
                  "&categoria=tutte" +
                  "&tipologia=tutte" +
                  "&giorno_day=" + day +
                  "&giorno_month=" + month +
                  "&giorno_year=" + year +
                  "&jaf_giorno_date_format=dd%2FMM%2Fyyyy&evn_visualizza=";

        var html = await HtmlUtil.DownloadHtmlAsync(url, false, CacheTypeEnum.ROOMTABLE);
        if (html.IsValid() == false) return new Tuple<List<HtmlNode>?, string>(null, "html invalid");

        var doc = new HtmlDocument();
        doc.LoadHtml(html.GetData());
        List<HtmlNode> nodes = new();

        var node = new HtmlNode(HtmlNodeType.Element, doc, 0)
        {
            InnerHtml = doc.DocumentNode.InnerHtml
        };
        nodes.Add(node);
        return new Tuple<List<HtmlNode>?, string>(nodes, string.Empty);
    }

    internal static int GetShiftSlotFromTime(DateTime time)
    {
        var shiftSlot = (time.Hour - 8) * 4;
        shiftSlot += time.Minute / 15;
        return shiftSlot;
    }
}