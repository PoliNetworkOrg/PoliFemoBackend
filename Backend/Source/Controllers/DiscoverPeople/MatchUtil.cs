#region

using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliNetwork.Db.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople;

#region

using DB = Database;

#endregion

public class MatchUtil
{
    private static string GenerateRandomHash(int length)
    {
        var randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var hashBytes = SHA256.HashData(randomBytes);

        var hashStringBuilder = new StringBuilder();
        foreach (var b in hashBytes) hashStringBuilder.Append(b.ToString("x2"));

        return hashStringBuilder.ToString()[..length];
    }

    public static ActionResult SetAnswerMatch(
        string fromUser,
        string toUser,
        bool yesOrNo,
        ControllerBase discoverPeopleController
    )
    {
        var sRandom = GenerateRandomHash(20);
        const string q =
            "INSERT IGNORE INTO PeopleDiscoverMatch " +
            "(from_person, to_person, answer, mn, ms) " +
            "VALUES " +
            "(SHA2(@p1,256)," +
            "SHA2(@p2,256)," +
            "@a," +
            "(SELECT COALESCE(MAX(ms), 0) + 1 FROM PeopleDiscoverMatch), " +
            "@ms" +
            ")";

        var dictionary = new Dictionary<string, object?>
        {
            { "@p1", fromUser },
            { "@p2", toUser },
            { "@a", yesOrNo },
            { "@ms", sRandom }
        };
        var i = DB.Execute(q, GlobalVariables.DbConfigVar, dictionary);
        var jObject = new JObject { { "r", i } };
        return discoverPeopleController.Ok(jObject);
    }
}