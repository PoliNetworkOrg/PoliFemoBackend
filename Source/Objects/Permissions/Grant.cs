using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Permissions;

public class Grant
{
    public Grant(string nameGrant, int? idObject)
    {
        if (idObject == -1)
            object_id = null;
        else
            object_id = idObject;
        grant = nameGrant;
    }

    public int? object_id { get; init; }

    public string grant { get; init; }

    public static List<JObject> GetFormattedPerms(IEnumerable<Grant> perms)
    {
        return perms.Select(t => new JObject
            { { nameof(grant), t.grant }, { nameof(object_id), t.object_id } }).ToList();
    }
}