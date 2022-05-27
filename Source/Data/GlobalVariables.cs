#region includes

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;
using System.IdentityModel.Tokens.Jwt;

#endregion

namespace PoliFemoBackend.Source.Data;

public static class GlobalVariables
{
    public static readonly DateTime start = DateTime.Now;
    public static readonly JObject? secrets = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("secrets.json"));
    public static DbConfig? DbConfigVar { get; set; }
    public static MySqlConnection? DbConnection { get; set; }
    public static JwtSecurityTokenHandler? TokenHandler { get; set; }
}