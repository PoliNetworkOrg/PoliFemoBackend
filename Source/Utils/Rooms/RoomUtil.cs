#region

using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Objects.Rooms;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class RoomUtil
{
    private const string RoomInfoUrls = "https://www7.ceda.polimi.it/spazi/spazi/controller/";

    internal static object GetAula(HtmlNode? node, IEnumerable<RoomOccupancyResultObject> roomOccupancyResultObjects,
        int shiftStop)
    {
        //Flag to indicate if the room has a power outlet (true/false)
        var pwr = RoomWithPower(node);
        var dove = node?.ChildNodes.First(x => x.HasClass("dove"));
        //Get Room name
        var nome = dove?.ChildNodes.First(x => x.Name == "a")?.InnerText.Trim();
        //Get Building name
        var edificio = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value.Split('-')[2].Trim();
        //Get address
        var indirizzo = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value.Split('-')[1].Trim();
        //get room link
        var info = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["href"]?.Value;

        var occupancies = new JObject();

        foreach (var roomOccupancyResultObject in roomOccupancyResultObjects.Where(x =>
                     x._timeOnly > TimeRoomUtil.GetTimeFromShiftSlot(shiftStop)))
        {
            if (occupancies.Children().Any() && occupancies.Children().Last().Last().ToString() ==
                roomOccupancyResultObject.RoomOccupancyEnum.ToString()) continue;
            occupancies.Add(roomOccupancyResultObject._timeOnly.ToString(),
                roomOccupancyResultObject.RoomOccupancyEnum.ToString());
        }

        //Builds room object 
        return new
        {
            name = nome, building = edificio, address = indirizzo, power = pwr, link = RoomInfoUrls + info,
            occupancies
        };
    }

    public static async Task<JObject?> GetRoomById(int id)
    {
        var url = RoomInfoUrls + "Aula.do?" +
                  "idaula=" + id;

        var html = await HtmlUtil.DownloadHtmlAsync(url);
        if (html.IsValid() == false) return null;
        /*
        example of property tag
        <td colspan="1" rowspan="1" style="width: 33%" class="ElementInfoCard1 jaf-card-element">
			<i>Codice vano</i>
			<br>&nbsp;LCF040800S042
		</td>
        (parsing doesn't work very well, regex++)
        */
        var fetchedHtml = html.GetData() ?? "";
        string[] fields = { "Sigla", "Capienza", "Edificio", "Indirizzo" };
        string[] names = { "name", "capacity", "building", "address" };
        //other fields include "Tipologia", "Indirizzo", "Dipartimento", "Codice vano", "Postazione per studenti disabili", ...
        var properties = new JObject();
        var propLen = fields.Length;
        for (var i = 0; i < propLen; i++)
        {
            var iTag = $@"<em>{fields[i]}</em>";
            var filter = new Regex($@"{iTag}.*?<br>.*?</td>", RegexOptions.Singleline);
            var match = filter.Match(fetchedHtml);
            if (match.Success)
                properties.Add(names[i], match.Value
                    .Replace(iTag, "")
                    .Replace("<br>", "")
                    .Replace("</td>", "")
                    .Replace("&nbsp;", "")
                    .Replace("\n", "").Trim()
                );
            else
                properties.Add(names[i], null);

            if (properties[names[i]]?.ToString() == "-")
                properties[names[i]] = null;
        }

        properties["building"] = properties["building"]?.ToString().Split('-')[0].Trim();
        var json = await File.ReadAllTextAsync("Other/Examples/roomsWithPower.json");
        var data = JObject.Parse(json);
        //Retrieving the list of IDs for the room with power outlets
        var list = data["rwp"]?.Select(x => (int)x).ToArray();
        properties["power"] = list != null && list.Contains(id);
        properties["capacity"] = int.Parse(properties["capacity"]?.ToString() ?? "-1");
        return properties;
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