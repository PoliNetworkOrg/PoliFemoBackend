#region includes

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Utils;
using System.Net;
using PoliFemoBackend.Source.Data;
using Database = PoliFemoBackend.Source.Utils.Database;


#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
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
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    // public async Task<ObjectResult> SearchGroups([BindRequired] string name)
    // {
    //     var json = await GroupsUtil.GetGroups();
    //     if (json == null)
    //     {
    //         return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" })
    //         { StatusCode = (int)HttpStatusCode.InternalServerError };
    //     }

    //     bool Filter(dynamic item)
    //     {
    //         return item["class"].ToString().ToLower().Contains(name.ToLower());
    //     }

    //     var filtered = GroupsUtil.Filter(json, (Func<dynamic, bool>)Filter);
    //     return Ok(filtered);
    // }

    public ObjectResult SearchGroupsDb(string name)
    {
        var results = Database.ExecuteSelect(
            "SELECT * FROM gruppo WHERE class = @name",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object>
            {
                { "name", name }
            });
            

        return Ok(results);
    }
}