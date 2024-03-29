#region

using System.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliNetwork.Db.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Calendar;

[ApiController]
[ApiExplorerSettings(GroupName = "Calendar")]
[Route("/calendar/search/range")]
public class SearchRangeOfDate : ControllerBase
{
    /// <summary>
    ///     Searches for available groups
    /// </summary>
    /// <param name="start" example="2022-09-12">Start date</param>
    /// <param name="end" example="2023-06-04">End date</param>
    /// <returns>A range of date and their typology</returns>
    /// <response code="200">Returns the array of date</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available date</response>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SearchDateDb(DateTime start, DateTime end)
    {
        var query =
            "SELECT DISTINCT Types.name, Days.giorno FROM Days, belongsTo, Types WHERE Days.giorno BETWEEN '@start' AND '@end' AND Days.giorno = belongsTo.giorno AND belongsTo.id_tipologia = Types.id_tipologia ORDER BY Days.giorno";
        query = query.Replace("@start", start.ToString("yyyy-MM-dd"));
        query = query.Replace("@end", end.ToString("yyyy-MM-dd"));

        var results = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar);

        if (results == null)
            return StatusCode(500);
        if (results.Rows.Count == 0)
            return NoContent();

        //crea oggetto Day in json

        var days = new JArray();
        foreach (DataRow row in results.Rows)
        {
            var day = new JObject
            {
                //evita duplicati
                { "date", ((DateTime)row["giorno"]).ToString("yyyy-MM-dd") }
            };

            //controllare se esiste già un day dentro a days
            var exists = days.Select(d => d.Value<string>("date") ?? "")
                .Any(x => x.Equals(day.Value<string>("date")));

            if (exists)
                continue;

            day.Add(
                "type",
                GetArrayString(results, ((DateTime)row["giorno"]).ToString("yyyy-MM-dd"))
            );
            days.Add(day);
        }

        var giorni = new JObject { { "giorni", days } };
        return Ok(giorni);
    }

    /// <summary>
    ///     METHOD RETURN A STRING WITH ALL TYPOLOGIES OF A DAY
    /// </summary>
    /// <returns></returns>
    private static JArray GetArrayString(DataTable results, string date)
    {
        var array = new JArray();
        foreach (DataRow row in results.Rows)
            //evita duplicati
            //compare 2 string
            if (date.Equals(((DateTime)row["giorno"]).ToString("yyyy-MM-dd")))
            {
                Console.WriteLine(row["name"]);
                array.Add(row["name"]);
            }

        return array;
    }
}
