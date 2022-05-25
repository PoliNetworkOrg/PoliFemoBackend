#region

using System.Text.Json;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

#endregion

namespace PoliFemoBackend.Source.Utils;


[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class DbConfig
{
    public string Database;
    public string Host;
    public string Password;
    public int Port;
    public string User;

    public static void InitializeDbConfig()
    {
        if (File.Exists(Constants.Constants.DbConfig))
        {
            try
            {
                var text = File.ReadAllText(Constants.Constants.DbConfig);
                GlobalVariables.DbConfig = JsonConvert.DeserializeObject<DbConfig>(text);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex);
            }

            if (GlobalVariables.DbConfig == null) GenerateDbConfigEmpty();
        }
        else
        {
            GenerateDbConfigEmpty();
        }

        GlobalVariables.DbConnection = new MySqlConnection(GlobalVariables.DbConfig?.GetConnectionString());
    }

    private static void GenerateDbConfigEmpty()
    {
        GlobalVariables.DbConfig = new DbConfig();
        var x = JsonConvert.SerializeObject(GlobalVariables.DbConfig);
        File.WriteAllText(Constants.Constants.DbConfig, x);
        Logger.WriteLine("Initialized DBConfig to empty!", LogSeverityLevel.CRITICAL);
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }

    public string GetConnectionString()
    {
        return "server='" + Host + "';user='" + User + "';database='" + Database + "';port=" + Port + ";password='" +
               Password + "'";
    }
    public static DbConfig DbConfig { get; set; }   
}



