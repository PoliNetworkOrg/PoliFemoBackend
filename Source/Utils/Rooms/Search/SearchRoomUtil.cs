using System.Collections;
using System.Data;
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

        var polimidailysituation = "polimidailysituation://" + sede + "/" +  hourStart?.ToString("yyyy-MM-dd");
        const string selectFromWebcacheWhereUrlLikeUrl = "SELECT * FROM WebCache WHERE url LIKE @url";
        var dictionary = new Dictionary<string, object?>
        {
            {"@url", polimidailysituation}
        };
        var q = Database.Database.ExecuteSelect(selectFromWebcacheWhereUrlLikeUrl, GlobalVariables.DbConfigVar, dictionary);

        if (q?.Rows.Count > 0)
        {
            return await ReturnFromCache(q);
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
            var r2 = FormatRoom(room);
            if (r2!=null)
                results.Add(r2);
        }

        SaveToCache(polimidailysituation, results);
        return new Tuple<JArray?, DoneEnum>(results, DoneEnum.DONE);
    }

    private static JObject? FormatRoom(object? room)
    {
        if (room == null) return null;

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

        return formattedRoom;
    }

    private static void SaveToCache(string polimidailysituation, IEnumerable results)
    {
        const string qi =
            "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW() + INTERVAL 2 DAYS)";
        var objects = new Dictionary<string, object?>
        {
            { "@url", polimidailysituation },
            { "@content", results.ToString() }
        };
        Database.Database.Execute(qi, GlobalVariables.DbConfigVar, objects);
    }

    private static async Task<Tuple<JArray?, DoneEnum>> ReturnFromCache(DataTable q)
    {
        var sq = q?.Rows[0]["content"]?.ToString();
        var jArray = new JArray();
        if (sq != null) jArray = JArray.Parse(sq);
        var tasks = (from JObject roomobj in jArray select Task.Run(() => { UpdateOccupancyRate(roomobj); })).ToList();
        await Task.WhenAll(tasks);
        return new Tuple<JArray?, DoneEnum>(jArray, DoneEnum.DONE);
    }

    private static void UpdateOccupancyRate(JObject roomobj)
    {
        roomobj["occupancy_rate"] =
            RoomOccupancyReport.GetReportedOccupancyJObject((uint)(roomobj["room_id"] ?? 1))?["occupancy_rate"];
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