#region

using System.IdentityModel.Tokens.Jwt;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Data;

public static class GlobalVariables
{
    public static readonly DateTime Start = DateTime.Now;
    public static JObject? Secrets = null;
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }
    public static JwtSecurityTokenHandler? TokenHandler { get; set; }
}