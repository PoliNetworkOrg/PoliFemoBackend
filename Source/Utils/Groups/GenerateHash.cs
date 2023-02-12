using System.Security.Cryptography;
using System.Text;

namespace PoliFemoBackend.Source.Utils.Groups;

public class GenerateHash
{
    public static string generatedId(string s)
    {
        var id = "";
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
            id = builder.ToString();
        }

        Console.WriteLine(s);
        return id;
    }
}