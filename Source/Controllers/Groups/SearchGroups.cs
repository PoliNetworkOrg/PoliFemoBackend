#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[Obsolete("This endpoint will be removed with no replacement. Please use the JSON file to search for groups.")]
[ApiController]
[ApiExplorerSettings(GroupName = "Groups")]
[Route("/groups")]
public class SearchGroupsController : ControllerBase
{
    /// <summary>
    ///     Search for groups by parameters
    /// </summary>
    /// <param name="name" example="Informatica">Group name</param>
    /// <param name="year" example="2022/2023">Year</param>
    /// <param name="degree" example="LT">Possible values: LT, LM, LU </param>
    /// <param name="type" example="C">Possible values: S, C, E</param>
    /// <param name="platform" example="TG">Possible values: WA, TG, FB</param>
    /// <param name="language" example="ITA">Possible values: ITA, ENG</param>
    /// <param name="office" example="Leonardo">Possible values: Bovisa, Como, Cremona, Lecco, Leonardo</param>
    /// <returns>An array of Group objects</returns>
    /// <response code="200">Request completed succesfully</response>
    /// <response code="500">Can't connect to server</response>
    [HttpGet]
    public ActionResult SearchGroupsDb(string name, string? year, string? degree, string? type, string? platform,
        string? language, string? office)
    {
        var d = new Dictionary<string, object?> { { "@name", "%" + name + "%" } };

        var query = "SELECT * FROM Groups WHERE class LIKE @name";
        if (year != null)
        {
            query += " AND year = @year";
            d.Add("@year", year);
        }

        if (degree != null)
        {
            query += " AND degree = @degree";
            d.Add("@degree", degree);
        }

        if (type != null)
        {
            query += " AND type_ = @type";
            d.Add("@type", type);
        }

        if (platform != null)
        {
            query += " AND platform = @platform";
            d.Add("@platform", platform);
        }

        if (language != null)
        {
            query += " AND language = @language";
            d.Add("@language", language);
        }

        if (office != null)
        {
            query += " AND office = @office;";
            d.Add("@office", office);
        }

        var results = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, d);

        var sg = JsonConvert.SerializeObject(results);
        HttpContext.Response.ContentType = "application/json";

        var ag = JsonConvert.DeserializeObject(sg) as JArray;

        var o = new
        {
            groups = ag ?? new JArray(),
            name,
            year,
            degree,
            type,
            platform,
            language,
            office
        };
        return Ok(o);
    }
}