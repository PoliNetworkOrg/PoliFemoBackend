namespace PoliFemoBackend.Source.Objects.Permission;

public class PermissionGrantObject
{
    public readonly string? id_object;
    public readonly string? name_grant;

    public PermissionGrantObject(string? name_grant, string? id_object)
    {
        this.name_grant = name_grant;
        this.id_object = id_object;
    }
}