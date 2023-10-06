#region

using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Objects.Permissions;

[Serializable]
public class Grant
{
    public Grant(string nameGrant, int? idObject)
    {
        object_id = idObject == -1 ? null : idObject;
        grant = nameGrant;
    }

    public int? object_id { get; init; }

    public string grant { get; init; }

    public static List<JObject> GetFormattedPerms(IEnumerable<Grant> perms)
    {
        return perms
            .Select(
                t => new JObject { { nameof(grant), t.grant }, { nameof(object_id), t.object_id } }
            )
            .ToList();
    }
}
