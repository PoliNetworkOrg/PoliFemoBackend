namespace PoliFemoBackend.Source.Utils;

public static class ApiVersionsManager
{
    private static readonly List<string> Versions = new();

    public static void AddVersion(string version)
    {
        Versions.Add(version);
    }

    public static List<string> ReadApiVersions()
    {
        return Versions;
    }
}