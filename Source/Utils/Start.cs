#region

using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class Start
{
    public static void StartThings()
    {
        try
        {
            GlobalVariables.SetSecrets(JsonConvert.DeserializeObject<JObject>(File.ReadAllText("secrets.json")));
            ProcessStartInfo processStartInfo = new ProcessStartInfo() { FileName = "/usr/bin/echo", Arguments = "$PF_VER", UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true };
            Process process = new Process() { StartInfo = processStartInfo };
            process.Start();
            GlobalVariables.LatestHash = process.StandardOutput.ReadToEnd();
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