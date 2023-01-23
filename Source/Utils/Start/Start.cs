#region

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.Start;

public static class Start
{
    public static void StartThings(bool useNews = true)
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

        if (useNews) {
            try
            {
                ThreadStartUtil.ThreadStartMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } else {
            Logger.WriteLine("--no-news flag found. We will not search for news.", LogSeverityLevel.Info);
        }
    }
}