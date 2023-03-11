using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Controllers.Rooms;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Utils.Rooms.Search;

public static class SearchRoomUtil
{
    public static async Task<Tuple<JArray?, DoneEnum>> SearchRooms(string sede, DateTime? hourStart, DateTime? hourStop)
    {
        hourStop = hourStop?.AddMinutes(-1);

        var q = Database.Database.ExecuteSelect("SELECT * FROM WebCache WHERE url LIKE @url", GlobalVariables.DbConfigVar, new Dictionary<string, object?>
        {
            {"@url", "polimidailysituation://" + hourStart?.ToString("yyyy-MM-dd")}
        }
        );

        if (q?.Rows.Count > 0)
        {
            var sq = q?.Rows[0]["content"]?.ToString();
            JArray jArray = new JArray();
            if (sq != null) jArray = JArray.Parse(sq);
            List<Task> tasks = new List<Task>();
            foreach (JObject roomobj in jArray)
            {
                tasks.Add(Task.Run(() =>
                {
                    roomobj["occupancy_rate"] = RoomOccupancyReport.GetReportedOccupancyJObject((uint)(roomobj["room_id"] ?? 1))?["occupancy_rate"];
                }));
            }
            await Task.WhenAll(tasks);
            return new Tuple<JArray?, DoneEnum>(jArray, DoneEnum.DONE);
        }

        var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, sede);
        if (t3 is null || t3.Count == 0) return new Tuple<JArray?, DoneEnum>(null, DoneEnum.ERROR);

        var htmlNode = t3[0];
        var t4 = FreeRoomsUtil.GetFreeRooms(htmlNode, hourStart, hourStop);
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

        Database.Database.Execute("INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW())", GlobalVariables.DbConfigVar, new Dictionary<string, object?>
        {
            {"@url", "polimidailysituation://" + hourStart?.ToString("yyyy-MM-dd")},
            {"@content", results.ToString()}
        }
        );
        return new Tuple<JArray?, DoneEnum>(results, DoneEnum.DONE);
    }

    internal static async Task<IActionResult> ReturnSearchResults(string sede, DateTime? hourStart, DateTime? hourStop,
        ControllerBase controllerBase)
    {
        var (jArrayResults, doneEnum) = await SearchRooms(sede, hourStart, hourStop);
        switch (doneEnum)
        {
            case DoneEnum.DONE:
                return controllerBase.Ok(new JObject(new JProperty("free_rooms", jArrayResults)));
            case DoneEnum.SKIPPED:
                return controllerBase.NoContent();
            default:
            case DoneEnum.ERROR:
            {
                const string text4 = "Errore nella consultazione del sito del polimi!";
                return new ObjectResult(new { error = text4 })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}