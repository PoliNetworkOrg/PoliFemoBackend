using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("/accounts/me/settings")]
public class AccountSettings : ControllerBase
{
    /// <summary>
    ///     Get the settings for the account
    /// </summary>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The key is incorrect</response>
    /// <response code="401">Invalid authentication</response>
    /// <response code="500">Can't connect to server</response>
    [Authorize]
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ObjectResult GetSettings()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub)) return BadRequest("");

        const string query = "SELECT expires_days FROM Users WHERE user_id = (SHA2(@sub, 256))";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub }
        };

        var r = DB.ExecuteSelect(query, GlobalVariables.DbConfigVar, parameters);
        var value = DB.GetFirstValueFromDataTable(r);
        if (value == null)
            return StatusCode(500, "");

        var jObject = new JObject
        {
            { "expire_in_days", int.Parse(value.ToString() ?? "0") }
        };

        return Ok(jObject);
    }

    /// <summary>
    ///     Change the settings for the account
    /// </summary>
    /// <response code="200">Request completed successfully</response>
    /// <response code="400">The key is incorrect</response>
    /// <response code="401">Invalid authentication</response>
    /// <response code="500">Can't connect to server</response>
    [Authorize]
    [HttpPost]
    public ObjectResult SetSettings([FromBody] JObject body)
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        var query = "";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub }
        };

        for (var i = 0; i < body.Count; i++)
        {
            var key = body.Properties().ElementAt(i).Name;
            var value = body.Properties().ElementAt(i).Value;
            switch (key)
            {
                case "expire_in_days":
                    if (value.Value<int>() < 30 || value.Value<int>() > 365 * 5)
                        return new BadRequestObjectResult(new JObject
                        {
                            { "error", "Invalid value. The number of days must be between 30 and 1825" }
                        });

                    query = "UPDATE Users SET expires_days = @v WHERE user_id = (SHA2(@sub, 256))";
                    parameters.Add("@v", value.Value<int>());
                    break;

                default:
                    return new BadRequestObjectResult(new JObject
                    {
                        { "error", "Invalid key" }
                    });
            }
        }

        var r = DB.Execute(query, GlobalVariables.DbConfigVar, parameters);
        return r != 1 ? StatusCode(500, "") : Ok("");
    }
}