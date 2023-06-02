using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Main;

public static class ProgramUtil
{
    internal static async Task OnChallengeMethod(JwtBearerChallengeContext context)
    {
        var json = new JObject
        {
            { "error", "Invalid token. Refresh your current access token or request a new authorization code" },
            { "reason", context.AuthenticateFailure?.Message }
        };
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";


        if (!context.Request.Headers.ContainsKey(Constants.Authorization))
            json["reason"] = "Missing Authorization header";
        else if (!context.Request.Headers[Constants.Authorization].ToString().StartsWith("Bearer "))
            json["reason"] = "Not a Bearer token";

        context.HandleResponse();
        await context.Response.WriteAsync(SampleNuGet.Utils.SerializeUtil.JsonToString(json) ?? "");
    }
}