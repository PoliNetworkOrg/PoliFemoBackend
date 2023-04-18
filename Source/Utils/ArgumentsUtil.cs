using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils;

public class ArgumentsUtil
{
    public bool UseNews = true;

    public ArgumentsUtil(IEnumerable<string> args)
    {
        GlobalVariables.BasePath = "/";
        foreach (var arg in args)

            ArgumentSingle(arg);
    }

    private void ArgumentSingle(string arg)
    {
        if (arg.Contains('='))
            KeyValuePairs(arg);
        else
            Flags(arg);
    }

    private void Flags(string arg)
    {
        //flags
        var value = arg[2..];
        UseNews = value switch
        {
            "no-news" => false,
            _ => UseNews
        };
    }

    private static void KeyValuePairs(string arg)
    {
        //key-value pairs
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
            case "no-db-setup":
                GlobalVariables.SkipDbSetup = value == "true";
                break;
        }
    }
}