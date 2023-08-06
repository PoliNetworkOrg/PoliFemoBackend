#region

using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils.Html;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class SingleRoomUtil
{
    public static async Task<JObject?> GetRoomById(int id)
    {
        var url = RoomUtil.RoomInfoUrls + "Aula.do?" +
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

    internal static bool RoomWithPower(HtmlNode? node)
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
}