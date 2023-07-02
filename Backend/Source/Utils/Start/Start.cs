﻿#region

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
            DbConfigUtil.InitializeDbConfig();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }


        try
        {
            ThreadStartUtil.ThreadStartMethod(useNews);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}