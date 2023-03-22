using System.Collections;
using System.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Utils.Rooms.Search;

public static class SearchRoomUtil
{
    public static async Task<Tuple<JArray?, DoneEnum>> SearchRooms(string? sede, DateTime? hourStart,
        DateTime? hourStop)
    {
        hourStop = hourStop?.AddMinutes(-1);
        string[] sedi;
        if (sede == null)
            sedi = new[] { "MIA", "MIB", "LCF", "MNI", "PCL" };
        else
            sedi = new[] { sede };
        var results = new JArray();

        foreach (var item in sedi)
        {
            var temp = new JArray();
            var polimidailysituation = "polimidailysituation://" + item + "/" + hourStart?.ToString("yyyy-MM-dd");
            const string selectFromWebcacheWhereUrlLikeUrl = "SELECT * FROM WebCache WHERE url LIKE @url";
            var dictionary = new Dictionary<string, object?>
            {
                { "@url", polimidailysituation }
            };
            var q = Database.Database.ExecuteSelect(selectFromWebcacheWhereUrlLikeUrl, GlobalVariables.DbConfigVar,
                dictionary);

            if (q?.Rows.Count > 0)
            {
                //add to results from cache
                var sq = q?.Rows[0]["content"].ToString();
                if (sq != null)
                {
                    var jArray = JArray.Parse(sq);
                    foreach (var jToken in jArray) results.Add(jToken);
                }

                continue;
            }

            var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, item);
            if (t3.Item1 is null || t3.Item1?.Count == 0)
                return new Tuple<JArray?, DoneEnum>(new JArray { t3.Item2 }, DoneEnum.ERROR);

            var htmlNode = t3.Item1?[0];
            var t4 = FreeRoomsUtil.GetFreeRooms(htmlNode, hourStart, hourStop);
            if (t4 is null || t4.Count == 0)
                return new Tuple<JArray?, DoneEnum>(null, DoneEnum.SKIPPED);


            foreach (var room in t4)
            {
                var r2 = FormatRoom(room);
                if (r2 != null)
                    temp.Add(r2);
            }


            Utils.Cache.SaveToCacheUtil.SaveToCache(polimidailysituation, temp);
            foreach (var jToken in temp) results.Add(jToken);
        }


        UpdateOccupancyRate(results);

        return new Tuple<JArray?, DoneEnum>(results, DoneEnum.DONE);
    }

    private static JObject? FormatRoom(object? room)
    {
        if (room == null) return null;

        var formattedRoom = JObject.FromObject(room);
        var roomLink = formattedRoom.GetValue("link");
        if (roomLink == null)
            return formattedRoom;

        var roomId = uint.Parse(roomLink.ToString().Split("idaula=")[1]);
        formattedRoom.Add(new JProperty("room_id", roomId));

        formattedRoom["occupancy_rate"] = null;

        return formattedRoom;
    }



    private static void UpdateOccupancyRate(JArray rooms)
    {
        var ids = new int[rooms.Count];
        var i = 0;
        foreach (JObject roomobj in rooms)
        {
            var id = int.Parse(roomobj["room_id"]?.ToString() ?? "0");
            if (roomobj?["room_id"] != null)
                ids[i++] = id;
        }

        var q = string.Format("SELECT room_id, SUM(x.w * x.rate)/SUM(x.w) " +
                              "FROM (" +
                              "SELECT room_id, TIMESTAMPDIFF(SECOND, NOW(), when_reported) w, rate " +
                              "FROM RoomOccupancyReports " +
                              "WHERE room_id in ({0}) AND when_reported >= @yesterday" +
                              ") x GROUP BY room_id", string.Join(",", ids));
        var dict = new Dictionary<string, object?>
        {
            { "@yesterday", DateTime.Now.AddDays(-1) }
        };
        var q2 = Database.Database.ExecuteSelect(q, GlobalVariables.DbConfigVar, dict);
        if (q2?.Rows.Count > 0)
            foreach (DataRow row in q2.Rows)
            foreach (JObject roomobj in rooms)
                if (roomobj["room_id"]?.ToString() == row[0].ToString())
                {
                    roomobj["occupancy_rate"] = (double)row[1];
                    break;
                }
    }


    internal static async Task<IActionResult> ReturnSearchResults(string? sede, DateTime? hourStart, DateTime? hourStop,
        ControllerBase controllerBase)
    {
        var (jArrayResults, doneEnum) = await SearchRooms(sede, hourStart, hourStop);
        return ReturnActionResult(controllerBase, doneEnum, jArrayResults);
    }

    public static IActionResult ReturnActionResult(ControllerBase controllerBase, DoneEnum doneEnum,
        IEnumerable? jArrayResults)
    {
        switch (doneEnum)
        {
            case DoneEnum.DONE:
                var jObject = new JObject(new JProperty("free_rooms", jArrayResults));
                return controllerBase.Ok(jObject);
            case DoneEnum.SKIPPED:
                return controllerBase.NoContent();
            default:
            case DoneEnum.ERROR:
            {
                const string text4 = "Errore nella consultazione del sito del polimi!";
                var error = text4 + " " + jArrayResults;
                return new ObjectResult(new { error })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}