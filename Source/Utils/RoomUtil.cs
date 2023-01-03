#region

using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class RoomUtil
{
    private const string ROOM_INFO_URLS = "https://www7.ceda.polimi.it/spazi/spazi/controller/";

    internal static List<object?>? GetFreeRooms(HtmlNode? table, DateTime start, DateTime stop)
    {
        if (table?.ChildNodes == null) return null;

        var shiftStart = GetShiftSlotFromTime(start);
        var shiftEnd = GetShiftSlotFromTime(stop);

        return (from child in table.ChildNodes
            where child != null
            select CheckIfFree(child, shiftStart, shiftEnd)
            into toAdd
            where toAdd != null
            select toAdd).ToList();
    }

    private static object? CheckIfFree(HtmlNode? node, int shiftStart, int shiftEnd)
    {
        if (node == null) return null;

        if (!node.GetClasses().Contains("normalRow")) return null;

        if (node.ChildNodes == null) return null;

        if (!node.ChildNodes.Any(x =>
                x.HasClass("dove")
                && x.ChildNodes != null
                && x.ChildNodes.Any(x2 => x2.Name == "a" && !x2.InnerText.ToUpper().Contains("PROVA"))
            ))
            return null;

        var roomFree = IsRoomFree(node, shiftStart, shiftEnd);
        return roomFree ? GetAula(node) : null;
    }

    private static bool IsRoomFree(HtmlNode? node, int shiftStart, int shiftEnd)
    {
        if (node?.ChildNodes == null) return true;

        var colsizetotal = 0;
        // the first two children are not time slots
        for (var i = 2; i < node.ChildNodes.Count; i++)
        {
            int colsize;
            // for each column, take it's span as the colsize
            if (node.ChildNodes[i].Attributes.Contains("colspan"))
                colsize = (int)Convert.ToInt64(node.ChildNodes[i].Attributes["colspan"].Value);
            else
                colsize = 1;

            // the time start in shifts for each column, is the previous total
            var vStart = colsizetotal;
            colsizetotal += colsize;
            var vEnd = colsizetotal; // the end is the new total (prev + colsize)

            // this is the trickery, if any column ends before the shift start or starts before
            // the shift end, then we skip
            if (vEnd < shiftStart || vStart > shiftEnd) continue;

            // if one of the not-skipped column represents an actual lesson, then return false,
            // the room is occupied
            if (!string.IsNullOrEmpty(node.ChildNodes[i].InnerHtml.Trim())) return false;
        }

        // if no lesson takes place in the room in the time window, the room is free (duh)
        return true;
    }

    private static object GetAula(HtmlNode? node)
    {
        //Flag to indicate if the room has a power outlet (true/false)
        var pwr = RoomWithPower(node);
        var dove = node?.ChildNodes.First(x => x.HasClass("dove"));
        //Get Room name
        var nome = dove?.ChildNodes.First(x => x.Name == "a")?.InnerText.Trim();
        //Get Building name
        var edificio = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value.Split('-')[2].Trim();
        //get room link
        var info = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["href"]?.Value;

        //Builds room object 
        return new { name = nome, building = edificio, power = pwr, link = ROOM_INFO_URLS + info };
    }

    private static bool RoomWithPower(HtmlNode? node)
    {
        var dove = node?.ChildNodes.First(x => x.HasClass("dove"));
        var a = dove?.ChildNodes.First(x => x.Name == "a");

        var aulaUrl = a?.Attributes["href"].Value;

        //Get the room id, in order to see whether it has power or not
        var idAula = int.Parse(aulaUrl?.Split('=').Last() ?? string.Empty);

        var json = File.ReadAllText("Other/Examples/roomsWithPower.json");
        var data = JObject.Parse(json);

        //Retrieving the list of IDs for the room with power outlets
        var list = data["rwp"]?.Select(x => (int)x).ToArray();

        //Checking whether the room has a power outlet
        return list != null && list.Contains(idAula);
    }

    private static int GetShiftSlotFromTime(DateTime time)
    {
        var shiftSlot = (time.Hour - 8) * 4;
        shiftSlot += time.Minute / 15;
        return shiftSlot;
    }


    internal static async Task<List<HtmlNode>?> GetDailySituationOnDate(DateTime date, string sede)
    {
        var day = date.Day;
        var month = date.Month;
        var year = date.Year;

        if (string.IsNullOrEmpty(sede)) return null;

        var url = "https://www7.ceda.polimi.it/spazi/spazi/controller/OccupazioniGiornoEsatto.do?" +
                  "csic=" + sede +
                  "&categoria=tutte" +
                  "&tipologia=tutte" +
                  "&giorno_day=" + day +
                  "&giorno_month=" + month +
                  "&giorno_year=" + year +
                  "&jaf_giorno_date_format=dd%2FMM%2Fyyyy&evn_visualizza=";

        var html = await HtmlUtil.DownloadHtmlAsync(url);
        if (html.IsValid() == false) return null;

        var doc = new HtmlDocument();
        doc.LoadHtml(html.GetData());

        var t1 = HtmlUtil.GetElementsByTagAndClassName(doc.DocumentNode, "", "BoxInfoCard", 1);

        //Get html node tbody (table) containing the rooms' daily situation requested by the query 
        var t3 = HtmlUtil.GetElementsByTagAndClassName(t1?[0], "", "scrollContent");
        return t3;
    }
}