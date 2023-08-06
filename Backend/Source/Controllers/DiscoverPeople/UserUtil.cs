using System.Data;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople;

public class UserUtil
{
    public static JObject GetUser(DataRow row)
    {
        var r = new JObject
        {
            ["user_id"] = row.ItemArray[0]?.ToString(),
            ["discover_bio"] = row.ItemArray[1]?.ToString(),
            ["discover_link"] = row.ItemArray.Length > 2 ? row.ItemArray[2]?.ToString() : null
        };
        return r;
    }
}