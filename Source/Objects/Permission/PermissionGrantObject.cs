using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Permission;

public class PermissionGrantObject
{
    private readonly string? id_object;
    private readonly string? name_grant;

    public PermissionGrantObject(string? name_grant, string? id_object)
    {
        this.name_grant = name_grant;
        this.id_object = id_object;
    }

    public static List<JObject> GetFormattedPerms(List<PermissionGrantObject> perms)
    {
        var formattedPerms = new List<JObject>();
        foreach(var t in perms){
            formattedPerms.Add(new JObject{
                { "grant", t.name_grant },
                { "object_id", t.id_object == "" ? null : t.id_object }
            });
        }

        return formattedPerms;
    }
}