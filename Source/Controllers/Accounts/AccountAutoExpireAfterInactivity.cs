﻿using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Objects.Threading;
using PoliFemoBackend.Source.Utils;
using PoliFemoBackend.Source.Utils.Account;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.Accounts;

[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Accounts")]
[Route("/accounts/me/autoexpire")]
public class AccountAutoExpireAfterInactivity : ControllerBase
{
    /// <summary>
    ///     Get the timespan for auto expire after inactivity
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public ObjectResult GetAutoExpireAfterInactivityTimeSpan()
    {
        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub)) return BadRequest("");

        const string query = "SELECT expireInactivity FROM USERS WHERE user_id = (SHA2(@sub, 256))";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub }
        };

        var r = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, parameters);
        var value = Database.GetFirstValueFromDataTable(r);
        if (value == null)
            return StatusCode(500, "");

        var timeSpan = (TimeSpan)value;
        return Ok(timeSpan);
    }

    /// <summary>
    ///     Set the timespan for auto expire after inactivity
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public ObjectResult SetAutoExpireAfterInactivityTimeSpan(TimeSpan timeSpan)
    {
        switch (timeSpan.TotalDays)
        {
            case < 30: //30 days
                return new BadRequestObjectResult("timeSpan too low");
            case > 365 * 5: // 5 years
                return new BadRequestObjectResult("timeSpan too high");
        }

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(sub)) return BadRequest("");

        const string query = "UPDATE USERS SET  expireInactivity = @t  WHERE user_id = (SHA2(@sub, 256))";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub },
            { "@t", timeSpan }
        };

        var r = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
        return r > 0 ? Ok("") : StatusCode(500, "");
    }

    /// <summary>
    ///     Loop in a thread that checks every day if someone is inactive and delete them
    /// </summary>
    /// <param name="threadWithAction"></param>
    public static void LoopCheckInactivity(ThreadWithAction threadWithAction)
    {
        const int timeToWait = 1000 * 60 * 60 * 24; //every day
        var count = 0;
        while (true)
        {
            try
            {
                var r = CheckInactivity();
                count += r ?? 0;

                threadWithAction.Partial.Add(r ?? 0);
                threadWithAction.Total = count;
            }
            catch (Exception ex)
            {
                threadWithAction.Failed++;
                Logger.WriteLine(ex, LogSeverityLevel.Error);
            }

            Thread.Sleep(timeToWait);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    /// <summary>
    ///     Check if users have been inactive for more than what is permitted by their settings.
    ///     Delete those users.
    /// </summary>
    /// <returns>How many users we deleted</returns>
    private static int? CheckInactivity()
    {
        const string q = "SELECT user_id FROM USERS " +
                         "WHERE (expireInactivity IS NOT NULL AND last_activity + expireInactivity >= NOW()) " +
                         "OR (expireInactivity IS NULL AND DATE_ADD(last_activity, INTERVAL 2 YEAR) >= NOW())";
        var d = Database.ExecuteSelect(q, null);

        return d?.Rows.Cast<DataRow>()
            .Count(dr => AccountDeletionUtil.DeleteAccountSingle(dr.ItemArray[0]?.ToString(), true));
    }
}