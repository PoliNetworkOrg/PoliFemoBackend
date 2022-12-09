namespace PoliFemoBackend.Source.Data;

public static class Constants
{
    public const string ArticlesUrl = "https://pastebin.com/raw/Giry1b7z";

    public const string GroupsUrl =
        "https://raw.githubusercontent.com/PoliNetworkOrg/polinetworkWebsiteData/main/groups.json";

    public const string DbConfig = "../config/dbconfig.json";

    public const string DataLogPath = "../logs/backend.log";

    public const string AzureClientId = "92602f24-dd8e-448e-a378-b1c575310f9d";

    public const string AzureScope = "api://92602f24-dd8e-448e-a378-b1c575310f9d/PoliFemoApi.Use";

    public const string AzureAuthority = "https://login.microsoftonline.com/95b7f038-c424-4961-ac49-74198cff1333/v2.0";

    public const string AzureAudience = "api://92602f24-dd8e-448e-a378-b1c575310f9d";

    public const string AzureIssuer = "https://sts.windows.net/0a17712b-6df3-425d-808e-309df28a5eeb/";

    public const string Authorization = "Authorization";
    public const double MaxRate = 5;
    public const double MinRate = 1;
}