#region

using System.Data;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Controllers.DiscoverPeople;

public class UserUtil
{
    public static JObject GetUser(DataRow row)
    {
        var r = new JObject
        {
            ["user_id"] = row.ItemArray[0]?.ToString(),
            ["discover_bio"] = row.ItemArray[1]?.ToString(),
            ["discover_link"] = row.ItemArray.Length > 2 ? row.ItemArray[2]?.ToString() : null,
            ["mn1"] = row.ItemArray.Length > 3 ? row.ItemArray[3]?.ToString() : null,
            ["ms1"] = row.ItemArray.Length > 4 ? row.ItemArray[4]?.ToString() : null,
            ["mn2"] = row.ItemArray.Length > 5 ? row.ItemArray[5]?.ToString() : null,
            ["ms2"] = row.ItemArray.Length > 6 ? row.ItemArray[6]?.ToString() : null
        };
        return r;
    }
}