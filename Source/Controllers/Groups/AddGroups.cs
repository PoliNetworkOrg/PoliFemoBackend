#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Permissions;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("/groups")]

public class AddGroupsController : ControllerBase
{
    /// <summary>
    ///     Add a new group
    /// </summary>
    /// <returns>An array of Group objects</returns>
    /// <response code="200">Request completed succesfully</response>
    /// <response code="500">Can't connect to server</response>
    [MapToApiVersion("1.0")]
    [Authorize]
    [HttpPost]
    public ObjectResult AddGroupsDb(Group group)
    {
        var d = new Dictionary<string, object?> { { "@name", group.name } };

        var query = "INSERT INTO Groups VALUES ( @name, ";

        //office
        if (group.office != null)
        {
            query += "@office,";
            d.Add("@office", group.office);
        }
        else
        {
            query += "null, ";
        }

        //id
        if (!string.IsNullOrEmpty(group.id))
        {
            query += "@id,";
            d.Add("@id", group.id);
        }

        //degree
        if (group.degree != null)
        {
            query += "@degree,";
            d.Add("@degree", group.degree);
        }
        else
        {
            query += "null, ";
        }

        //school
        if (group.school != null)
        {
            query += "@school, ";
            d.Add("@school", group.school);
        }
        else
        {
            query += "null, ";
        }

        //id_link
        if (!string.IsNullOrEmpty(group.link_id))
        {
            query += "@id_link,";
            d.Add("@id_link", group.link_id);
        }
        

        //language
        if (!string.IsNullOrEmpty(group.language))
        {
            query += "@language,";
            d.Add("@language", group.language);
        }

        //type
        if (group.type != null)
        {
            query += "@type,";
            d.Add("@type", group.type);
        }
        else
        {
            query += "null, ";
        }

        //year
        if (group.year != null)
        {
            query += "@year, ";
            d.Add("@year", group.year);
        }
        else
        {
            query += "null, ";
        }

        //platform
        if (group.platform != null)
        {
            query += "@platform,";
            d.Add("@platform", group.platform);
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

        var results = Database.Execute(query, GlobalVariables.DbConfigVar, d);

        return results == 0 ?
            StatusCode(500, new { message = "Can't connect to server" }) :
            new CreatedResult("/groups", new { message = "Group added", status = 200 });
    }
}