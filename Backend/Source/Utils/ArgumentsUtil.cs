#region

using PoliFemoBackend.Source.Data;
using PoliNetwork.Core.Utils.LoggerNS;
using LogLevel = PoliNetwork.Core.Utils.LoggerNS.LogLevel;

#endregion

namespace PoliFemoBackend.Source.Utils;

public class ArgumentsUtil
{
    public readonly bool UseNews = true;
    public readonly bool UseRoomsSearch = true;

    public ArgumentsUtil(IEnumerable<string> args)
    {
        GlobalVariables.BasePath = "/";
        foreach (var arg in args)
            //key-value pairs
            if (arg.Contains('='))
            {
                var split = arg.Split('=');
                var key = split[0][2..];
                var value = split[1];

                switch (key)
                {
                    case "base-path":
                        GlobalVariables.BasePath = value;
                        break;
                    case "log-level":
                        var logConfig = GetLogConfig(value);
                        PoliNetwork.Core.Data.GlobalVariables.DefaultLogger.SetLogConfig(logConfig);
                        break;
                    case "no-db-setup":
                        GlobalVariables.SkipDbSetup = value == "true";
                        break;
                }
            }
            //flags
            else
            {
                var value = arg[2..];
                UseNews = value switch
                {
                    "no-news" => false,
                    _ => UseNews
                };
                UseRoomsSearch = value switch
                {
                    "no-rooms" => false,
                    _ => UseRoomsSearch
                };
            }
        if (GlobalVariables.LogLevel == 0)
        {
            var logConfig = GetLogConfig("3");
            PoliNetwork.Core.Data.GlobalVariables.DefaultLogger.SetLogConfig(logConfig);
        }
    }

    private static LogConfig GetLogConfig(string value)
    {
        GlobalVariables.LogLevel = int.Parse(value);
        var logLevel = (LogLevel)GlobalVariables.LogLevel;
        var logConfig = new LogConfig(logLevel, true, "logs");
        return logConfig;
    }
}
