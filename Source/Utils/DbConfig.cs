#region

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Data;

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
    public static DbConfig? DbConfigVar { get; set; }

    public static void InitializeDbConfig()
    {
        if (File.Exists(Constants.DbConfig))
        {
            try
            {
                var text = File.ReadAllText(Constants.DbConfig);
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
        FileInfo file = new(Constants.DbConfig);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, x);
        Logger.WriteLine("Initialized DBConfig to empty!", LogSeverityLevel.Critical);
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }

    public string GetConnectionString()
    {
        return "server='" + Host + "';user='" + User + "';database='" + Database + "';port=" + Port + ";password='" +
               Password + "'";
    }
}