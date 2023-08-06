#region

using System.Data;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Data;
using PoliNetwork.Db.Objects;
using PoliNetwork.Db.Utils;
using DB = PoliNetwork.Db.Utils.Database;
using GlobalVariables = PoliNetwork.Core.Data.GlobalVariables;

#endregion

namespace PoliFemoBackend.Source.Utils.Database;

public static class DbConfigUtilPoliFemo
{
    public static DbConfig? DbConfigVar { get; private set; }


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
                if (DbConfigVar != null) DbConfigVar.Logger = GlobalVariables.DefaultLogger;
                Data.GlobalVariables.DbConfigVar = DbConfigVar;
            }
            catch (Exception ex)
            {
                GlobalVariables.DefaultLogger.Error(ex.ToString());
            }

            if (DbConfigVar == null)
                GenerateDbConfigEmpty();
        }
        else
        {
            GenerateDbConfigEmpty();
        }


        var connectionString = DbConfigUtils.GetConnectionString(Data.GlobalVariables.DbConfigVar);
        if (string.IsNullOrEmpty(connectionString)) 
            return;
        
        Data.GlobalVariables.DbConnection = new MySqlConnection(connectionString);
        try
        {
            Data.GlobalVariables.DbConnection.Open();
            if (Data.GlobalVariables.DbConnection.State == ConnectionState.Open)
                GlobalVariables.DefaultLogger.Info(
                    "Connection to db on start works! Performing table checks...");

            if (Data.GlobalVariables.SkipDbSetup is null or false)
            {
                var sql = File.ReadAllText(Constants.SqlCommandsPath);
                DB.ExecuteSelect(sql, Data.GlobalVariables.DbConfigVar);
            }

            GlobalVariables.DefaultLogger.Info(
                "Table checks completed! Starting application...");
        }
        catch (Exception ex)
        {
            GlobalVariables.DefaultLogger.Emergency(
                "An error occurred while initializing the database. Check the details and try again.");
            GlobalVariables.DefaultLogger.Emergency(ex.Message);

            Environment.Exit(1);
        }
    }

    private static void GenerateDbConfigEmpty()
    {
        DbConfigVar = new DbConfig(GlobalVariables.DefaultLogger);
        Data.GlobalVariables.DbConfigVar = DbConfigVar;
        var x = JsonConvert.SerializeObject(DbConfigVar);
        FileInfo file = new(Constants.DbConfig);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, x);
        GlobalVariables.DefaultLogger.Info("Initialized DBConfig to empty!");
        throw new Exception("Database failed to initialize, we generated an empty file to fill");
    }

    public static DbConfig? GetDbConfigNew()
    {
        return Data.GlobalVariables.GetDbConfig();
    }
}