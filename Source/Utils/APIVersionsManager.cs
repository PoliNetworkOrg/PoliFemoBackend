namespace PoliFemoBackend.Source.Utils;

public static class APIVersionsManager
{
    private static List<string> versions = new();

    public static void AddVersion(string version)
    {
        versions.Add(version);

        return;
    }

    public static List<string> ReadAPIVersions()
    {
        return versions;
    }
}