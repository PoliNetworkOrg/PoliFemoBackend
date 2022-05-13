using System.Net;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils;

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[Route("[controller]")]
public class FreeRoomsController : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public async Task<ObjectResult> SearchFreeRooms(DateTime day, string sede, DateTime hourStart, DateTime hourStop)
    {
        var t3 = await Utils.RoomUtil.GetDailySituationOnDate(day, sede);
        if (t3 is null || t3.Count == 0)
        {
            const string text4 = "Errore nella consultazione del sito del polimi!";
            return Ok(text4);
        }

        var t4 = Utils.RoomUtil.GetFreeRooms(t3[0], hourStart, hourStop);
        if (t4 == null || t4.Count == 0)
        {
            const string text3 = "Nessuna aula libera trovata!";
            return Ok(text3);
        }

        var result = t4.Aggregate("", (current, room) => current + room + "\n");
        return Ok(result);
    }


}

