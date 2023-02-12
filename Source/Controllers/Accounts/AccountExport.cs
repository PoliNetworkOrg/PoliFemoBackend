#region

using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Permissions;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("accounts/me/export")]
[Authorize]
public class AccountExportController : ControllerBase
{
    /// <summary>
    ///     Get a file with all of the user's data
    /// </summary>
    /// <response code="200">Request completed successfully</response>
    /// <response code="500">Can't connect to the server</response>
    [HttpGet]
    public FileContentResult ExportData()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);

        var query = "SELECT user_id, last_activity, account_type FROM Users WHERE user_id = SHA2(@sub, 256)";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub }
        };
        var q = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, parameters);
        var lastActivity = DateTime.Parse(q?.Rows[0]["last_activity"]?.ToString() ?? "");
        var id = q?.Rows[0]["user_id"]?.ToString() ?? "";
        var accountType = q?.Rows[0]["account_type"]?.ToString() ?? "";

        query = "SELECT * FROM RoomOccupancyReports WHERE user_id = SHA2(@sub, 256)";
        q = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, parameters);
        var occupancyReports = q?.Rows;
        var roc = new JArray();
        if (occupancyReports == null)
            return FileExport(id, lastActivity, accountType, sub, roc);

        foreach (DataRow row in occupancyReports)
            roc.Add(JObject.FromObject(new
            {
                room_id = row["room_id"],
                when_reported = row["when_reported"],
                rate = row["rate"]
            }));
        return FileExport(id, lastActivity, accountType, sub, roc);
    }

    private FileContentResult FileExport(string id, DateTime lastActivity, string accountType, string? sub,
        JArray roc)
    {
        return File(Encoding.UTF8.GetBytes(JObject.FromObject(new
        {
            id,
            last_activity = lastActivity.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture),
            account_type = accountType,
            permissions = Grant.GetFormattedPerms(AuthUtil.GetPermissions(sub)),
            room_occupancy_reports = roc
        }).ToString()), "application/json", id + ".json");
    }
}