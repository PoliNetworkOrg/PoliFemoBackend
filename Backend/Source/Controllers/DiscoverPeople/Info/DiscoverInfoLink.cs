using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople.Info;

public class DiscoverInfoLink : ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("/discoverpeople/info/setLink/{stringLink}")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SetLink(string stringLink)
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : SetLink(tempSub, stringLink, this);
    }

    [HttpGet]
    [Authorize]
    [Route("/discoverpeople/info/getLink/")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult GetLink()
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : GetLinkUtil(tempSub, this);
    }

    private static ActionResult GetLinkUtil(string tempSub, ControllerBase discoverInfo)
    {
        const string q = "SELECT discover_link FROM Users WHERE user_id = @id";
        var i = DB.ExecuteSelect(q, GlobalVariables.DbConfigVar, new Dictionary<string, object?>()
        {
            { "@id", tempSub }
        });
        if (i == null)
            return discoverInfo.NotFound();

        var value = i.Rows[0].ItemArray[0]?.ToString();
        return discoverInfo.Ok(new JObject() { { "link", value } });
    }

    private static ActionResult SetLink(string tempSub, string stringLink, ControllerBase discoverInfo)
    {
        const string q = "UPDATE Users SET discover_link = @link WHERE user_id = @id";
        var i = DB.Execute(q, GlobalVariables.DbConfigVar, new Dictionary<string, object?>()
        {
            { "@id", tempSub },
            { "@link", stringLink }
        });
        return discoverInfo.Ok(i);
    }
}