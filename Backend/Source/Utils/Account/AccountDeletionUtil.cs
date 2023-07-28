#region

using PoliFemoBackend.Source.Data;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Account;

public static class AccountDeletionUtil
{
    /// <summary>
    ///     Delete an user.
    /// </summary>
    /// <param name="sub">sub (user)</param>
    /// <param name="hashed">if sub is alreadt hashed or not</param>
    /// <returns>True if deleted successfully</returns>
    public static bool DeleteAccountSingle(string? sub, bool hashed)
    {
        if (string.IsNullOrEmpty(sub))
            return false;

        const string queryNotHashed = "SELECT deleteUser(SHA2(@sub, 256))";
        const string queryHashed = "SELECT deleteUser(@sub)";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub }
        };

        var queryToRun = hashed ? queryHashed : queryNotHashed;
        var r = DB.ExecuteSelect(queryToRun, GlobalVariables.DbConfigVar, parameters);
        return r != null;
    }
}