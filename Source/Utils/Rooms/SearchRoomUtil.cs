using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Utils.Rooms;

public static class SearchRoomUtil
{
    public static async Task<Tuple<JArray?,DoneEnum> > SearchRooms(string sede, DateTime hourStart, DateTime hourStop)
    {
        hourStop = hourStop.AddMinutes(-1);
        var t3 = await RoomUtil.GetDailySituationOnDate(hourStart, sede);
        if (t3 is null || t3.Count == 0)
        {
            return new Tuple<JArray?, DoneEnum>(null, DoneEnum.ERROR);
        }

        var t4 = RoomUtil.GetFreeRooms(t3[0], hourStart, hourStop);
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
                var roomId = int.Parse(roomLink.ToString().Split("idaula=")[1]);
                formattedRoom.Add(new JProperty("room_id", roomId));
            }

            results.Add(formattedRoom);
        }

        return new Tuple<JArray?, DoneEnum>(results, DoneEnum.DONE);

    }
}