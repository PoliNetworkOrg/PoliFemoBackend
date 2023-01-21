using System.Security.Cryptography;
namespace PoliFemoBackend.Source.Utils.Groups;


   public class GenerateHash{
    public static string generatedId(string s){
        string id = "";
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(s));
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            id = builder.ToString();
        }
        Console.WriteLine(s);
        return id;
    }
    
}




