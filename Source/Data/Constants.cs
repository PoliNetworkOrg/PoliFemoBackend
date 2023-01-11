namespace PoliFemoBackend.Source.Data;

public static class Constants
{
    public const string ArticlesUrl = "https://pastebin.com/raw/Giry1b7z";

    public const string GroupsUrl =
        "https://raw.githubusercontent.com/PoliNetworkOrg/polinetworkWebsiteData/main/groups.json";

    public const string DbConfig = ConfigPath + "/dbconfig.json";

    public const string DataLogPath = LogsPath + "/backend.log";

    public const string ConfigPath = "./config";

    public const string DataPath = "./data";

    public const string LogsPath = "./logs";

    public const string AzureClientId = "92602f24-dd8e-448e-a378-b1c575310f9d";

    public const string AzureScope = "api://92602f24-dd8e-448e-a378-b1c575310f9d/PoliFemoApi.Use";

    public const string AzureAuthority = "https://login.microsoftonline.com/common/v2.0";

    public const string AzureOrgIssuer = "https://login.microsoftonline.com/0a17712b-6df3-425d-808e-309df28a5eeb/v2.0";

    public const string AzureCommonIssuer =
        "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";

    public const string Authorization = "Authorization";
    public const double MaxRate = 5;
    public const double MinRate = 1;

    public static class Permissions
    {
        public const string PermissionsConst = "permissions";
    }
}