#region

using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils.Database;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class DbConfig
{
    public string? DatabaseName;
    public string? Database;
    public string? Host;
    public string? Password;
    public int Port;
    public string? User;
    public static DbConfig? DbConfigVar { get; set; }

    public static void InitializeDbConfig(bool ignoreInitialScript = false)
    {
        ;
        if (!Directory.Exists(Constants.ConfigPath)) Directory.CreateDirectory(Constants.ConfigPath);

        const string configDbconfigJson = Constants.DbConfig;
        if (File.Exists(configDbconfigJson))
        {
            try
            {

                var text = File.ReadAllText(configDbconfigJson);
                DbConfigVar = JsonConvert.DeserializeObject<DbConfig>(text);
                DbConfigVar?.FixName();
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
                Logger.WriteLine("Connection to db on start works! Performing table checks...");

            if (ignoreInitialScript == false)
            {
                var sql = File.ReadAllText(Constants.SqlCommandsPath);
                Utils.Database.Database.ExecuteSelect(sql, GlobalVariables.DbConfigVar);
            }

            Logger.WriteLine("Table checks completed! Starting application...");
        }
        catch (Exception ex)
        {
            Logger.WriteLine("An error occurred while initializing the database. Check the details and try again.",
                LogSeverityLevel.Critical);
            Logger.WriteLine(ex.Message, LogSeverityLevel.Critical);
            Environment.Exit(1);
        }
    }

    private void FixName()
    {
        if (string.IsNullOrEmpty(this.DatabaseName))
            this.DatabaseName = this.Database;
        
        if (string.IsNullOrEmpty(this.Database))
            this.Database = this.DatabaseName;

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
            ? "server='" + Host + "';user='" + User + "';database='" + DatabaseName + "';port=" + Port
            : "server='" + Host + "';user='" + User + "';database='" + DatabaseName + "';port=" + Port + ";password='" +
              Password + "'";
    }

    public static DbConfig? GetDbConfigNew()
    {
        return GlobalVariables.GetDbConfig();
    }
}