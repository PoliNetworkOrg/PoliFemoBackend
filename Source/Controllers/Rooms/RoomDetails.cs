#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[ApiExplorerSettings(GroupName = "Rooms")]
[Route("/rooms/{id:int}")]
public class RoomDetailsController : ControllerBase
{
    /// <summary>
    ///     gets room details by id
    /// </summary>
    /// <param name="id" example="4635">Room id</param>
    /// <returns>Returns room details by id(name, address, capacity, building, power) </returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="500">Can't connect to poli servers</response>
    
    [HttpGet]
    public async Task<ObjectResult> getRoomDetails(int id){
        var room = await RoomUtil.getRoomById(id);
        if (room is null)
        {
            const string text4 = "Errore nella consultazione del sito del polimi!";
            return new ObjectResult(new { error = text4 })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
        return new ObjectResult(room);
    }
}