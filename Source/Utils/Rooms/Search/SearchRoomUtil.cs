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
        var sedi = new[] { "MIA", "MIB", "LCF", "MNI", "PCL" };
        var results = new JArray();

        foreach (var item in sedi)
        {
            var temp= new JArray();
            var polimidailysituation = "polimidailysituation://" + item + "/" + hourStart?.ToString("yyyy-MM-dd");
            const string selectFromWebcacheWhereUrlLikeUrl = "SELECT * FROM WebCache WHERE url LIKE @url";
            var dictionary = new Dictionary<string, object?>
            {
            {"@url", polimidailysituation}
            };
            var q = Database.Database.ExecuteSelect(selectFromWebcacheWhereUrlLikeUrl, GlobalVariables.DbConfigVar, dictionary);

            if (q?.Rows.Count > 0)
            {
                //add to results from cache
                var sq = q?.Rows[0]["content"].ToString();
                if (sq != null)
                {
                    var jArray = JArray.Parse(sq);
                    foreach (var jToken in jArray)
                    {
                        results.Add(jToken);
                    }
                }
                continue;
            }

            var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, item);
            if (t3.Item1 is null || t3.Item1?.Count == 0)
                return new Tuple<JArray?, DoneEnum>(new JArray() { t3.Item2 }, DoneEnum.ERROR);

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




            SaveToCache(polimidailysituation, temp);
            results.Add(temp);
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

        return formattedRoom;
    }

  

    private static void SaveToCache(string polimidailysituation, IEnumerable results)
    {
        try
        {
            const string qi =
                "INSERT INTO WebCache (url, content, expires_at) VALUES (@url, @content, NOW() + INTERVAL 2 DAY)";
            var objects = new Dictionary<string, object?>
            {
                { "@url", polimidailysituation },
                { "@content", results.ToString() }
            };
            Database.Database.Execute(qi, GlobalVariables.DbConfigVar, objects);
        }
        catch (Exception ex)
        {
            Logger.WriteLine(ex);
        }
    }

    private static async Task<Tuple<JArray?, DoneEnum>> ReturnFromCache(DataTable? q)
    {
        var sq = q?.Rows[0]["content"].ToString();
        var jArray = new JArray();
        if (sq != null) jArray = JArray.Parse(sq);
       
        return new Tuple<JArray?, DoneEnum>(jArray, DoneEnum.DONE);
    }

    private static void UpdateOccupancyRate(JArray rooms)
    {
        JArray ids = new JArray();
        foreach (JObject roomobj in rooms)
        {
        
            ids.Append(roomobj?.First["room_id"] ?? 1);
        }
        const string q = "SELECT room_id, SUM(x.w * x.rate)/SUM(x.w) " +
                        "FROM (" +
                        "SELECT TIMESTAMPDIFF(SECOND, NOW(), when_reported) w, rate " +
                        "FROM RoomOccupancyReports " +
                        "WHERE room_id in @room_id AND when_reported >= @yesterday" +
                        ") x ";
        var dict = new Dictionary<string, object?>
        {
            { "@room_id", ids },
            { "@yesterday", DateTime.Now.AddDays(-1) }
        };
        var q2 = Database.Database.ExecuteSelect(q, GlobalVariables.DbConfigVar, dict);
        if (q2?.Rows.Count > 0)
        {
            foreach(JObject roomobj in rooms)
            {
                foreach (DataRow row in q2.Rows)
                {
                    if (roomobj["room_id"] == row[0])
                        roomobj["occupancy_rate"] = (float)row[1];
                }
            }
        }
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
                var error = text4 + " " + jArrayResults;
                return new ObjectResult(new {error})
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}