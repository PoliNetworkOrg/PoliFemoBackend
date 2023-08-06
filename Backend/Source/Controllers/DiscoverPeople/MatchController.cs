using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using DB = PoliNetwork.Db.Utils.Database;


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
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : SetAnswerMatch(tempSub, id, true, this);
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
        return string.IsNullOrEmpty(tempSub) ? new EmptyResult() : SetAnswerMatch(tempSub, id, false, this);
    }

    private static ActionResult SetAnswerMatch(string fromUser, string toUser, bool yesOrNo,
        ControllerBase discoverPeopleController)
    {
        const string q = "INSERT IGNORE INTO PeopleDiscoverMatch (from_person, to_person, answer) VALUES (@p1,@p2,@a)";
        var i = DB.Execute(q, GlobalVariables.DbConfigVar, new Dictionary<string, object?>()
        {
            { "@p1", fromUser },
            { "@p2", toUser },
            { "@a", yesOrNo }
        });
        return discoverPeopleController.Ok(new JObject() { { "r", i } });
    }


    private static JArray? GetMatched(string tempSub)
    {
        const string q = "SELECT user_id, discover_bio, discover_link " +
                         "FROM Users u " +
                         "WHERE u.user_id IN (SELECT p1.to_person FROM PeopleDiscoverMatch p1 WHERE p1.from_person = @id AND p1.answer = TRUE AND p1.to_person IN (" +
                         "SELECT p2.from_person FROM PeopleDiscoverMatch p2 WHERE p2.from_person = p1.to_person AND p2.to_person = @id AND p2.answer = TRUE" +
                         "))";
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