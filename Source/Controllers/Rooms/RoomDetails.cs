#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/{id:int}")]
public class RoomDetailsController : ControllerBase
{
    /// <summary>
    ///     Get room details by ID
    /// </summary>
    /// <param name="id" example="4635">Room ID</param>
    /// <returns>Room name, address, capacity, building, power</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="500">Can't connect to poli servers</response>
    [HttpGet]
    public async Task<ObjectResult> GetRoomDetails(int id)
    {
        var room = await RoomUtil.getRoomById(id);
        if (room is not null) 
            return new ObjectResult(room);
        
        const string text4 = "Errore nella consultazione del sito del polimi!";
        return new ObjectResult(new { error = text4 })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

    }
}