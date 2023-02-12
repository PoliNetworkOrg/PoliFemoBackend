using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Accounts;
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("/accounts/me/autoexpire")]
public class AccountAutoExpireAfterInactivity: ControllerBase
{
    [Authorize]
    [HttpGet]
    public ObjectResult GetAutoExpireAfterInactivityTimeSpan()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub))
        {
            return BadRequest("");
        }

        const string query = "SELECT expireInactivity FROM USERS WHERE user_id = (SHA2(@sub, 256))";
        var parameters = new Dictionary<string, object?>
        {
            {"@sub", sub}
        };

        var r = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, parameters);
        var value = Database.GetFirstValueFromDataTable(r);
        if (value == null)
            return StatusCode(500, "");

        var timeSpan = (TimeSpan)value;
        return Ok(timeSpan);
    }
    
    [Authorize]
    [HttpPost]
    public ObjectResult SetAutoExpireAfterInactivityTimeSpan(TimeSpan timeSpan)
    {
        switch (timeSpan.TotalDays)
        {
            case < 30: //30 days
                return new BadRequestObjectResult("timeSpan too low");
            case > 365*5: // 5 years
                return new BadRequestObjectResult("timeSpan too high");
        }

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub))
        {
            return BadRequest("");
        }

        const string query = "UPDATE USERS SET  expireInactivity = @t  WHERE user_id = (SHA2(@sub, 256))";
        var parameters = new Dictionary<string, object?>
        {
            {"@sub", sub},
            {"@t", timeSpan}
        };

        var r = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
        return r > 0 ?   Ok("") : StatusCode(500, "");

    }
}