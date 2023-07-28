using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Data;
using PoliNetwork.Db.Utils;
using DB = PoliNetwork.Db.Utils.Database;

namespace PoliFemoBackend.Source.Utils.Database;

public static class DbConfigUtil
{
    public static DbConfig? DbConfigVar { get; set; }

    
    public static void InitializeDbConfig()
    {

        
        if (!Directory.Exists(Constants.ConfigPath)) 
            Directory.CreateDirectory(Constants.ConfigPath);

        const string configDbconfigJson = Constants.DbConfig;
        if (File.Exists(configDbconfigJson))
        {
            try
            {
                var text = File.ReadAllText(configDbconfigJson);
                DbConfigVar = JsonConvert.DeserializeObject<DbConfig>(text);
                DbConfigVar?.FixName();
                if (DbConfigVar != null)
                {
                    DbConfigVar.Logger = PoliNetwork.Core.Data.Variables.DefaultLogger;
                }
                GlobalVariables.DbConfigVar = DbConfigVar;
            }
            catch (Exception ex)
            {
                PoliNetwork.Core.Data.Variables.DefaultLogger.Error(ex.ToString());
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
                PoliNetwork.Core.Data.Variables.DefaultLogger.Info("Connection to db on start works! Performing table checks...");

            if (GlobalVariables.SkipDbSetup is null or false)
            {
                var sql = File.ReadAllText(Constants.SqlCommandsPath);
                DB.ExecuteSelect(sql, GlobalVariables.DbConfigVar);
            }

            PoliNetwork.Core.Data.Variables.DefaultLogger.Info("Table checks completed! Starting application...");
        }
        catch (Exception ex)
        {
            
            PoliNetwork.Core.Data.Variables.DefaultLogger.Emergency("An error occurred while initializing the database. Check the details and try again.");
            PoliNetwork.Core.Data.Variables.DefaultLogger.Emergency(ex.Message);
            
            Environment.Exit(1);
        }
    }
    
    private static void GenerateDbConfigEmpty()
    {
        DbConfigVar = new DbConfig(PoliNetwork.Core.Data.Variables.DefaultLogger);
        GlobalVariables.DbConfigVar = DbConfigVar;
        var x = JsonConvert.SerializeObject(DbConfigVar);
        FileInfo file = new(Constants.DbConfig);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, x);
        PoliNetwork.Core.Data.Variables.DefaultLogger.Info("Initialized DBConfig to empty!");
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }
    
    public static DbConfig? GetDbConfigNew()
    {
        return GlobalVariables.GetDbConfig();
    }
}