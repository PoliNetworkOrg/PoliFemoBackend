#region

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Start;

public static class Start
{
    public static void StartThings()
    {
        try
        {
            GlobalVariables.SetSecrets(JsonConvert.DeserializeObject<JObject>(File.ReadAllText(Constants.SecretJson)));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            DbConfig.InitializeDbConfig();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        try
        {
            ThreadStartUtil.ThreadStartMethod();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}