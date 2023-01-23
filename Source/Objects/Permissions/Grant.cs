using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Permissions;

public class Grant
{
    public int? object_id { get; set; }

    public string grant { get; set; }

    public Grant(string nameGrant, int? idObject)
    {
        grant = nameGrant;
        object_id = idObject;
    }

    public static List<JObject> GetFormattedPerms(IEnumerable<Grant> perms)
    {
        return perms.Select(t => new JObject
            { { "grant", t.grant }, { "object_id", t.object_id == null ? null : t.object_id } }).ToList();
    }
}