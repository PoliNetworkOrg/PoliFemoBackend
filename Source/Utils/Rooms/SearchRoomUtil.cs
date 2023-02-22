using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Rooms;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class SearchRoomUtil
{
    public static async Task<Tuple<JArray?, DoneEnum>> SearchRooms(string sede, DateTime? hourStart, DateTime? hourStop)
    {
        hourStop = hourStop?.AddMinutes(-1);

        var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, sede);
        if (t3 is null || t3.Count == 0) return new Tuple<JArray?, DoneEnum>(null, DoneEnum.ERROR);

        var htmlNode = t3[0];
        var t4 = RoomUtil.GetFreeRooms(htmlNode, hourStart, hourStop);
        if (t4 is null || t4.Count == 0)
            return new Tuple<JArray?, DoneEnum>(null, DoneEnum.SKIPPED);

        var results = new JArray();
        foreach (var room in t4)
        {
            if (room == null) continue;

            var formattedRoom = JObject.FromObject(room);
            var roomLink = formattedRoom.GetValue("link");
            if (roomLink != null)
            {
                var roomId = uint.Parse(roomLink.ToString().Split("idaula=")[1]);
                formattedRoom.Add(new JProperty("room_id", roomId));


                try
                {
                    var reportedOccupancyJObject = RoomOccupancyReport.GetReportedOccupancyJObject(roomId);
                    formattedRoom["occupancy_rate"] =
                        reportedOccupancyJObject?["occupancy_rate"];
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex);
                }
            }

            results.Add(formattedRoom);
        }

        return new Tuple<JArray?, DoneEnum>(results, DoneEnum.DONE);
    }
}