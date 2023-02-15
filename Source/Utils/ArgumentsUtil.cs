using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils;

public class ArgumentsUtil
{
    public readonly bool UseNews = true;

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
                        GlobalVariables.LogLevel = int.Parse(value);
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
            }
    }
}