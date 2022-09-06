#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("v{version:apiVersion}/groups/search")]
[Route("/groups/search")]
public class SearchGroupsController : ControllerBase
{
    /// <summary>
    ///     Searches for available groups
    /// </summary>
    /// <param name="name" example="Informatica">Group name</param>
    /// <param name="year" example="2022">Year</param>
    /// <param name="degree" example="LT">Possible values: LT, LM, LU </param>
    /// <param name="type" example="C">Possible values: S, C, E</param>
    /// <param name="platform" example="TG">Possible values: WA, TG, FB</param>
    /// <param name="language" example="ITA">Possible values: ITA, ENG</param>
    /// <param name="office" example="Leonardo">Possible values: Bovisa, Como, Cremona, Lecco, Leonardo</param>
    /// <returns>An array of free groups</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    // public async Task<ObjectResult> SearchGroups([BindRequired] string name, string? year, string? degree, string? type,
    //     string? platform, string? language, string? office)
    // {
    //     var json = await GroupsUtil.GetGroups();
    //     if (json == null)
    //     {
    //         return GroupsUtil.ErrorInRetrievingGroups();
    //     }

    //     //filtra per i parametri
    //     bool Filter(dynamic item)
    //     {
    //         return item["class"].ToString().ToLower().Contains(name.ToLower()) &&
    //                (string.IsNullOrEmpty(year) || item.year.ToString().ToLower().Contains(year.ToLower())) &&
    //                (string.IsNullOrEmpty(type) || item.type.ToString().ToLower().Contains(type.ToLower())) &&
    //                (string.IsNullOrEmpty(degree) || item.degree.ToString().ToLower().Contains(degree.ToLower())) &&
    //                (string.IsNullOrEmpty(platform) ||
    //                 item.platform.ToString().ToLower().Contains(platform.ToLower())) &&
    //                (string.IsNullOrEmpty(language) ||
    //                 item.language.ToString().ToLower().Contains(language.ToLower())) &&
    //                (string.IsNullOrEmpty(office) || item.office.ToString().ToLower().Contains(office.ToLower()));
    //     }

    //     var filtered = GroupsUtil.Filter(json, (Func<dynamic, bool>)Filter);
    //     return Ok(filtered);
    // }
    public ActionResult SearchGroupsDb(string name, string? year, string? degree, string? type, string? platform,
        string? language, string? office)
    {
        var d = new Dictionary<string, object?> { { "@name", name } };

        var query = "SELECT * FROM Groups WHERE class LIKE '%@name%'";
        if (year != null)
        {
            query += " AND year = '@year'";
            d.Add("@year", year);
        }

        if (degree != null)
        {
            query += " AND degree = '@degree'";
            d.Add("@degree", degree);
        }

        if (type != null)
        {
            query += " AND type_ = '@type'";
            d.Add("@type", type);
        }

        if (platform != null)
        {
            query += " AND platform = '@platform'";
            d.Add("@platform", platform);
        }

        if (language != null)
        {
            query += " AND language_ = '@language'";
            d.Add("@language", language);
        }

        if (office != null)
        {
            query += " AND office = '@office';";
            d.Add("@office", office);
        }

        var results = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, d);

        //if results is null
        if (results == null) return GroupsUtil.ErrorInRetrievingGroups();

        if (results.Rows.Count == 0) return NoContent();

        var sg = JsonConvert.SerializeObject(results);
        HttpContext.Response.ContentType = "application/json";

        var ag = JsonConvert.DeserializeObject(sg) as JArray;

        var o = new JObject { { "groups", ag } };
        return Ok(o);
    }
}