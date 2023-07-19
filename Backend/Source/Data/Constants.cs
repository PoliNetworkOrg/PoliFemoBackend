using PoliNetwork.Core.Utils.LoggerNS;

namespace PoliFemoBackend.Source.Data;

public static class Constants
{
    public const string DbConfig = ConfigPath + "/dbconfig.json";

    public const string DataLogPath = LogsPath + "/backend.log";

    public const string SecretJson = ConfigPath + "/secrets.json";

    public const string ConfigPath = "/config";

    public const string DataPath = "./data";

    public const string LogsPath = "./logs";

    public static readonly string SqlCommandsPath = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "./Backend/Other/DB/DBPolifemo.sql" : "Other/DB/DBPolifemo.sql";

    public const string AzureClientId = "a06b160b-8d5d-4be2-b452-ea3b768998ed";

    public const string AzureAuthority = "https://login.microsoftonline.com/common/v2.0";

    public const string AzurePolimiIssuer =
        "https://login.microsoftonline.com/0a17712b-6df3-425d-808e-309df28a5eeb/v2.0";

    public const string AzurePoliNetworkIssuer =
        "https://login.microsoftonline.com/7f8cafc8-4314-4070-9744-fe02f91bcb21/v2.0";

    public const string AzureCommonIssuer =
        "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";

    public const string AssetsUrl = "https://polinetworkorg.github.io/PoliFemo/assets/";

    public const string Authorization = "Authorization";
    public const double MaxRate = 5;
    public const double MinRate = 1;

    public static class Permissions
    {
        public const string ManagePermissions = "permissions";
    }
}