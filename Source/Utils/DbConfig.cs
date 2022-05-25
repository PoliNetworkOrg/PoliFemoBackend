#region includes

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PoliNetworkBot_CSharp.Code.Data;

#endregion

namespace PoliFemoBackend.Source.Utils;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class DbConfig
{
    public string? Database;
    public string? Host;
    public string? Password;
    public int Port;
    public string? User;

    public static void InitializeDbConfig()
    {
        if (File.Exists(Constants.Constants.DbConfig))
        {
            try
            {
                var text = File.ReadAllText(Constants.Constants.DbConfig);
                DbConfigVar = JsonConvert.DeserializeObject<DbConfig>(text);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex);
            }

            if (DbConfigVar == null) GenerateDbConfigEmpty();
        }
        else
        {
            GenerateDbConfigEmpty();
        }


        GlobalVariables.DbConnection = new MySqlConnection(GlobalVariables.DbConfigVar?.GetConnectionString());
    }

    private static void GenerateDbConfigEmpty()
    {
        DbConfigVar = new DbConfig();
        var x = JsonConvert.SerializeObject(DbConfigVar);
        File.WriteAllText(Constants.Constants.DbConfig, x);
        Logger.WriteLine("Initialized DBConfig to empty!", LogSeverityLevel.CRITICAL);
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }

    public string GetConnectionString()
    {
        return "server='" + Host + "';user='" + User + "';database='" + Database + "';port=" + Port + ";password='" + Password + "'";
    }
    public static DbConfig? DbConfigVar { get; set; }   
}



