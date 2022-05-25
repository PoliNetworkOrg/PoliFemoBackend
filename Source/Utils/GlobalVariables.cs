#region includes

using MySql.Data.MySqlClient;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliNetworkBot_CSharp.Code.Data;

public static class GlobalVariables
{
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }   
}