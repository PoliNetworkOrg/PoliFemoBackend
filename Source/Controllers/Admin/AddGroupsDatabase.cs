#region includes

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class AddGroupsController : ControllerBase
{
    /// <summary>
    ///     Add groups on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    

    public ObjectResult AddGroupsDb()
    {
            var json = JsonConvert.SerializeObject(GroupsUtil.GetGroups()); //get groups from url
            //get index_data from json
               var index_data = JsonConvert.DeserializeObject<dynamic>(json);
            for (int i = 0; i < index_data.Count; i++)
            {
                 try
                 {
                    if (index_data[i].degree == null)
                    {
                         index_data[i].degree = "";
                    }
                    if (index_data[i].school == null)
                    {
                         index_data[i].school = "";
                    }
                    if (index_data[i].year == null)
                    {
                         index_data[i].year = "";
                    }
                    if (index_data[i].permanentId == null)
                    {
                         index_data[i].permanentId = "";
                    }
                    if (index_data[i].linkfunzionante == null)
                    {
                         index_data[i].linkfunzionante = "";
                    }
                    
                    var query = "INSERT IGNORE INTO gruppo VALUES (" + index_data[i]["class"] + ", '" + index_data[i]["office"] + "', " + index_data[i]["id"] + ", '" + index_data[i]["degree"] + "','" + index_data[i]["school"] + "'," + index_data[i]["id_link"] + ",'" + index_data[i]["language"] + "','" + index_data[i]["type"] + "', '" + index_data[i]["year"] + "', '" + index_data[i]["platform"] + "', '" + index_data[i]["permanentId"] + "', '" + index_data[i]["LastUpdateInviteLinkTime"] + "', '" + index_data[i]["linkfunzionante"] + "');";
                    var queryreplace = query.Replace("''", "NULL");
                    var results = Database.ExecuteSelect(queryreplace, GlobalVariables.DbConfigVar);

                 }
                 catch (System.Exception)
                 {
                      
                      throw;
                 }
            }
          return Ok(index_data);
    }
}



