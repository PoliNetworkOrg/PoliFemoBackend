#region

using System.Data;
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
public class MatchController : ControllerBase
{
    [HttpPost]
    [Authorize]
    [Route("/discoverpeople/match/setYes/{id}")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SetAnswerMatchYes(string id)
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : MatchUtil.SetAnswerMatch(tempSub, id, true, this);
    }


    [HttpGet]
    [Authorize]
    [Route("/discoverpeople/match/get")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult GetMatched()
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        if (string.IsNullOrEmpty(tempSub))
            return new EmptyResult();
        var answerMatchYes = GetMatched(tempSub);
        return Ok(answerMatchYes);
    }

    [HttpPost]
    [Authorize]
    [Route("/discoverpeople/match/setNo/{id}")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SetAnswerMatchNo(string id)
    {
        var tempSub = AuthUtil.GetSubjectFromHttpRequest(Request);
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : MatchUtil.SetAnswerMatch(tempSub, id, false, this);
    }


    private static JArray? GetMatched(string tempSub)
    {
        /*
            tu = {tempSub} (@id)

            u.id  = altro
            p1.from = tu
            p1.to = altro
            p2.from = altro
            p1.to = tu

            u.id = p1.to
            p1.to = p2.from
            p1.from = @id
            p1.from = p2.to

         */
        const string q =
            "SELECT u.user_id, u.discover_bio, u.discover_link, p1.mn as mn1, p1.ms as ms1, p2.mn as mn2, p2.ms as ms2 " +
            "FROM Users u, PeopleDiscoverMatch p1, PeopleDiscoverMatch p2 " +
            "WHERE u.user_id = p1.to_person " +
            "AND p1.from_person = SHA2(@id,256) " +
            "AND p1.answer = TRUE " +
            "AND p1.to_person = p2.from_person " +
            "AND p2.from_person = p1.to_person " +
            "AND p2.to_person = SHA2(@id,256) " +
            "AND p2.answer = TRUE";

        var dictionary = new Dictionary<string, object?>
        {
            { "@id", tempSub }
        };
        var results = DB.ExecuteSelect(q, GlobalVariables.DbConfigVar, dictionary);

        if (results == null)
            return null;

        var jArray = new JArray();
        foreach (DataRow variable in results.Rows)
        {
            var j = UserUtil.GetUser(variable);
            jArray.Add(j);
        }

        return jArray;
    }
}