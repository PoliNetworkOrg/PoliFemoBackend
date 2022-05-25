namespace PoliFemoBackend.Source.Utils;

public static class APIVersionsManager
{
    private static readonly List<string> versions = new();

    public static void AddVersion(string version)
    {
        versions.Add(version);
    }

    public static List<string> ReadAPIVersions()
    {
        return versions;
    }
}