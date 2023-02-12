using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Account;

public static class AccountDeletionUtil
{
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
        var r = Database.Database.ExecuteSelect(queryToRun, GlobalVariables.DbConfigVar, parameters);
        return r != null;
    }
}