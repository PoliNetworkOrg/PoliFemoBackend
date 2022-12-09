using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Permission;

public class PermissionGrantObject
{
    private readonly string? _idObject;
    private readonly string? _nameGrant;

    public PermissionGrantObject(string? nameGrant, string? idObject)
    {
        _nameGrant = nameGrant;
        _idObject = idObject;
    }

    public static List<JObject> GetFormattedPerms(IEnumerable<PermissionGrantObject> perms)
    {
        return perms.Select(t => new JObject
            { { "grant", t._nameGrant }, { "object_id", t._idObject == "" ? null : t._idObject } }).ToList();
    }
}