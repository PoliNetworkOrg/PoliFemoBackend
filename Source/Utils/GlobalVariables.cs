#region includes

using System.IdentityModel.Tokens.Jwt;
using MySql.Data.MySqlClient;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class GlobalVariables
{
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }
    public static JwtSecurityTokenHandler? TokenHandler { get; set; }

}