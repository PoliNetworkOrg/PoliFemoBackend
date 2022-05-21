#region

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[Route("/groups/byname")]
public class GroupsByName : ControllerBase
{
    /// <summary>
    ///     Checks for available groups
    /// </summary>
    /// <param name="name" example="Informatica">Group name</param>
    /// <returns>An array of free groups</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [HttpGet]
    [HttpPost]
    public async Task<ObjectResult> SearchGroupByName([BindRequired] string name)
    {
        var json = await GroupsUtil.GetGroups();
        if (json == null)
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" })
                { StatusCode = (int)HttpStatusCode.InternalServerError };

        bool Filter(dynamic item)
        {
            return item["class"].ToString().ToLower().Contains(name.ToLower());
        }

        var filtered = GroupsUtil.Filter(json, (Func<dynamic, bool>)Filter);
        return Ok(filtered);
    }
}