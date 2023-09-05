#region

using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Rooms;

#endregion

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class ExtractHtmlRoomUtil
{
    internal static object? GetAula(HtmlNode? node, IEnumerable<RoomOccupancyResultObject> roomOccupancyResultObjects)
    {
        //Flag to indicate if the room has a power outlet (true/false)
        var pwr = SingleRoomUtil.HasPower(node);
        var dove = node?.ChildNodes.First(x => x.HasClass("dove"));
        //Get Room name
        var nome = dove?.ChildNodes.First(x => x.Name == "a")?.InnerText.Trim();

        // Some rooms are deactivated (in particular when using CRG), so we skip them
        if (dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value == "-") return null;

        //Get Building name
        var edificio = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value.Split('-')[2].Trim();
        //Get address
        var indirizzo = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["title"]?.Value.Split('-')[1].Trim();
        //get room link
        var info = dove?.ChildNodes.First(x => x.Name == "a")?.Attributes["href"]?.Value;

        var occupancies = GetOccupanciesJObject(roomOccupancyResultObjects);

        //Builds room object 
        return new
        {
            name = nome,
            building = edificio,
            address = indirizzo,
            power = pwr,
            link = RoomUtil.RoomInfoUrls + info,
            occupancies
        };
    }

    private static JObject GetOccupanciesJObject(IEnumerable<RoomOccupancyResultObject> roomOccupancyResultObjects)
    {
        var occupancies = new JObject();
        foreach (var roomOccupancyResultObject in roomOccupancyResultObjects)
        {
            var jObject = new JObject
            {
                ["status"] = roomOccupancyResultObject.RoomOccupancyEnum.ToString()
            };
            if (roomOccupancyResultObject.RoomOccupancyEnum == RoomOccupancyEnum.OCCUPIED)
                jObject.Add("text", roomOccupancyResultObject.text);
            var propertyName = roomOccupancyResultObject.TimeOnly.ToString();
            occupancies.Add(propertyName, jObject);
        }

        return occupancies;
    }
}