#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople;

[ApiController]
[ApiExplorerSettings(GroupName = "DiscoverPeople")]
public class DiscoverPeopleController : ControllerBase
{
    /// <summary>
    ///     Discover people
    /// </summary>
    /// <returns>A JSON object of people</returns>
    /// <response code="200">Request completed successfully</response>
    /// <response code="404">No available people</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    [Authorize]
    [Route("/discoverpeople/random")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult GetRandomPeople()
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(tempSub))
            return new EmptyResult();

        var a = NewPeople(tempSub);
        return a == null ? NotFound() : Ok(a);
    }


    private static JObject? NewPeople(string tempSub)
    {
        var results = DB.ExecuteSelect(
            "SELECT user_id, discover_bio " +
            "FROM Users " +
            "WHERE user_id NOT IN (SELECT to_person PeopleDiscoverMatch WHERE from_person = @id) " +
            "AND discover_bio IS NOT NULL " +
            "AND discover_bio != '' " +
            "AND discover_link IS NOT NULL " +
            "AND discover_link != '' " +
            "ORDER BY RAND()  LIMIT 10",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@id", tempSub }
            });

        var row = results?.Rows[0];
        return row == null ? null : UserUtil.GetUser(row);
    }
}