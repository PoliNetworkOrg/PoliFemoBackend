using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Groups.ModifyGroup;

public static class ModifyGroupUtil
{
    internal static ObjectResult ModifyGroupMethod(JObject ob, string id, ControllerBase modifyGroupsController)
    {
        var d = new Dictionary<string, object?> { { "@id", id } };

        var query = BuildQueryUtil.BuildQuery(ob, id, d);

        try
        {
            var results = Database.Database.Execute(query, GlobalVariables.DbConfigVar, d);
        }
        catch (Exception)
        {
            return modifyGroupsController.StatusCode(500, new { message = "Server error" });
        }

        return modifyGroupsController.Ok("");
    }
}