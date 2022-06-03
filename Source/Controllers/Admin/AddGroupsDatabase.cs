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
    /// Add groups on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Group Added</response>
    /// <response code="500">Can't connect to server or Group not Added</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    

    public ObjectResult AddGroupsDb(string name, string? year, string id, string? degree, string? type, string? platform, string language, string? office, string? school, string id_link)
    {

          var d = new Dictionary<string, object> { { "@name", name } };

          var query = "INSERT IGNORE INTO gruppo VALUES ( '@name', ";

          //office
          if (office != null)
          {
               query += "'@office',";
               d.Add("@office", office);
          }
          else
          {
               query += "null, ";
          }

          //id
          if (id != null)
          {
               query += "'@id',";
               d.Add("@id", id);
          }

          //degree
          if (degree != null)
          {
               query += "'@degree',";
               d.Add("@degree", degree);
          }
          else
          {
               query += "null, ";
          }

          //school
          if (school != null)
          {
               query += "'@school', ";
               d.Add("@school", school);
          }
          else
          {
               query += "null, ";
          }

          //id_link
          if (id_link != null)
          {
               query += "'@id_link',";
               d.Add("@id_link", id_link);
          }

          //language
          if (language != null)
          {
               query += "'@language',";
               d.Add("@language", language);
          }

          //type
          if (type != null)
          {
               query += "'@type',";
               d.Add("@type", type);
          }
          else
          {
               query += "null, ";
          }

          //year
          if (year != null)
          {
               query += "'@year', ";
               d.Add("@year", year);
          }
          else
          {
               query += "null, ";
          }

          //platform
          if (platform != null)
          {
               query += "'@platform',";
               d.Add("@platform", platform);
          }
          else
          {
               query += "null, ";
          }

          //permanent id
          query += "null, ";

          //Last update date
          query += "now(), ";

          //Link Funzionante
          query += "'Y');";
          
          //Console.WriteLine(query);
          var results = Database.Execute(query, GlobalVariables.DbConfigVar, d);
          
          if (results == 0)
          {
               return new ObjectResult(new { message = "Group NOT Added", status = 500 });
          }
          else
          {
               return new ObjectResult(new { message = "Group Added", status = 200 });
          }
    }
}



