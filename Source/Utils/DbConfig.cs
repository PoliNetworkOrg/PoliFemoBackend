#region

using System.Data;
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
        if (!Directory.Exists("../config/")) Directory.CreateDirectory("../config/");

        if (File.Exists(Constants.DbConfig))
        {
            try
            {
                var text = File.ReadAllText(Constants.DbConfig);
                DbConfigVar = JsonConvert.DeserializeObject<DbConfig>(text);
                GlobalVariables.DbConfigVar = DbConfigVar;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex);
            }

            if (DbConfigVar == null)
                GenerateDbConfigEmpty();
        }
        else
        {
            GenerateDbConfigEmpty();
        }


        GlobalVariables.DbConnection = new MySqlConnection(GlobalVariables.DbConfigVar?.GetConnectionString());
        try
        {
            GlobalVariables.DbConnection.Open();
            if (GlobalVariables.DbConnection.State == ConnectionState.Open)
                Console.WriteLine("Connection to db on start works!");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private static void GenerateDbConfigEmpty()
    {
        DbConfigVar = new DbConfig();
        GlobalVariables.DbConfigVar = DbConfigVar;
        var x = JsonConvert.SerializeObject(DbConfigVar);
        FileInfo file = new(Constants.DbConfig);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, x);
        Logger.WriteLine("Initialized DBConfig to empty!", LogSeverityLevel.Critical);
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }

    public string GetConnectionString()
    {
        return string.IsNullOrEmpty(Password)
            ? "server='" + Host + "';user='" + User + "';database='" + Database + "';port=" + Port
            : "server='" + Host + "';user='" + User + "';database='" + Database + "';port=" + Port + ";password='" +
              Password + "'";
    }
}