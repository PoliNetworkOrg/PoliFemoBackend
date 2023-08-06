#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople.Info;

public class DiscoverInfoBio : ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("/discoverpeople/info/setBio/{stringBio}")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SetBio(string stringBio)
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : SetBio(tempSub, stringBio, this);
    }

    [HttpGet]
    [Authorize]
    [Route("/discoverpeople/info/getBio/")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult GetBio()
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : GetBioUtil(tempSub, this);
    }

    private static ActionResult GetBioUtil(string tempSub, ControllerBase discoverInfo)
    {
        const string q = "SELECT discover_bio FROM Users WHERE user_id = SHA2(@id,256)";
        var i = DB.ExecuteSelect(q, GlobalVariables.DbConfigVar, new Dictionary<string, object?>
        {
            { "@id", tempSub }
        });
        if (i == null)
            return discoverInfo.NotFound();

        var value = i.Rows[0].ItemArray[0]?.ToString();
        return discoverInfo.Ok(new JObject { { "bio", value } });
    }

    private static ActionResult SetBio(string tempSub, string stringBio, ControllerBase discoverInfo)
    {
        const string q = "UPDATE Users SET discover_bio = @bio WHERE user_id = SHA2(@id,256)";
        var i = DB.Execute(q, GlobalVariables.DbConfigVar, new Dictionary<string, object?>
        {
            { "@id", tempSub },
            { "@bio", stringBio }
        });
        return discoverInfo.Ok(i);
    }
}