using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils;

public class ArgumentsUtil {
    public bool useNews = true;

    public ArgumentsUtil(string[] args)  {
        GlobalVariables.BasePath = "/";
        foreach (var arg in args) {
            //key-value pairs
            if (arg.Contains("=")) {
                var split = arg.Split('=');
                var key = split[0].Substring(2);
                var value = split[1];

                switch (key) {
                    case "base-path":
                        GlobalVariables.BasePath = value;
                        break;
                    case "log-level":
                        GlobalVariables.LogLevel = int.Parse(value);
                        break;
                }
            }
            //flags
            else {
                var value = arg.Substring(2);
                switch (value) {
                    case "no-news":
                        useNews = false;
                        break;
                }
            }
        }
    }
}