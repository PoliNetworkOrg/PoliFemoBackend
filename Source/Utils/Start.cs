﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils;

public static class Start
{
    public static void StartThings()
    {
        try
        {
            GlobalVariables.Secrets = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("secrets.json"));
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
    }
}