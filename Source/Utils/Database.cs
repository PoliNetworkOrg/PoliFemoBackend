#region

using System.Data;
using MySql.Data.MySqlClient;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class Database
{
    // ReSharper disable once UnusedMember.Global
    public static int Execute(string query, DbConfig? dbConfig, Dictionary<string, object>? args = null)
    {
        if (dbConfig == null) return default;

        Logger.WriteLine(query, LogSeverityLevel.DatabaseQuery); //todo metti gli args

        var connection = new MySqlConnection(dbConfig.GetConnectionString());


        if (args != null)
            foreach (var (key, value) in args)
                query = query.Replace(key, value.ToString());

        var cmd = new MySqlCommand(query, connection);

        if (args != null)
            foreach (var (key, value) in args)
                cmd.Parameters.AddWithValue(key, value);

        OpenConnection(connection);

        int? numberOfRowsAffected = null;
        if (connection.State == ConnectionState.Open) numberOfRowsAffected = cmd.ExecuteNonQuery();

        connection.Close();
        return numberOfRowsAffected ?? -1;
    }

    public static DataTable? ExecuteSelect(string query, DbConfig? dbConfig, Dictionary<string, object>? args = null)
    {
        if (dbConfig == null) return default;

        if (args != null)
            foreach (var (key, value) in args)
                query = query.Replace(key, value.ToString());

        Logger.WriteLine(query, LogSeverityLevel.DatabaseQuery);
        var connection = new MySqlConnection(dbConfig.GetConnectionString());

        var cmd = new MySqlCommand(query, connection);

        if (args != null)
            foreach (var (key, value) in args)
                cmd.Parameters.AddWithValue(key, value);

        OpenConnection(connection);

        var adapter = new MySqlDataAdapter
        {
            SelectCommand = cmd
        };


        if (connection.State != ConnectionState.Open)
            return default;

        var ret = new DataSet();
        adapter.Fill(ret);

        adapter.Dispose();
        connection.Close();
        return ret.Tables[0];
    }

    private static void OpenConnection(IDbConnection connection)
    {
        if (connection.State != ConnectionState.Open) connection.Open();
    }

    // ReSharper disable once UnusedMember.Global
    internal static object? GetFirstValueFromDataTable(DataTable dt)
    {
        try
        {
            return dt.Rows[0].ItemArray[0];
        }
        catch
        {
            return null;
        }
    }

    // ReSharper disable once UnusedMember.Global
    public static long? GetIntFromColumn(DataRow dr, string columnName)
    {
        var o = dr[columnName];
        if (o is null or DBNull) return null;

        try
        {
            return Convert.ToInt64(o);
        }
        catch
        {
            return null;
        }
    }
}