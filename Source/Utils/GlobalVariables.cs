#region includes

using MySql.Data.MySqlClient;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class GlobalVariables
{
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }
}