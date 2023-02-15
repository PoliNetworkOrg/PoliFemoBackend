using System.Security.Cryptography;
using System.Text;

namespace PoliFemoBackend.Source.Utils.Groups;

public static class GenerateHash
{
    public static string GeneratedId(string s)
    {
        var id = "";
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));
            var builder = new StringBuilder();
            foreach (var t in bytes)
                builder.Append(t.ToString("x2"));

            id = builder.ToString();
        }

        Console.WriteLine(s);
        return id;
    }
}