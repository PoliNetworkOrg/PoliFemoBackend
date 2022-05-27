#region includes

using MySql.Data.MySqlClient;
using PoliFemoBackend.Source.Utils;
using System.IdentityModel.Tokens.Jwt;

#endregion

namespace PoliFemoBackend.Source.Data;

public static class GlobalVariables
{
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }
    public static JwtSecurityTokenHandler? TokenHandler { get; set; }
}