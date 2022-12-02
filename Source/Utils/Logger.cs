#region

using System.Globalization;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class Logger
{
    private static readonly object LogFileLock = new();


    public static void WriteLine(object? log, LogSeverityLevel logSeverityLevel = LogSeverityLevel.Info)
    {
        if (log == null || string.IsNullOrEmpty(log.ToString())) return;

        try
        {
            Console.WriteLine(logSeverityLevel + " | " + log);
            var log1 = log.ToString();
            Directory.CreateDirectory("../data/");

            if (!File.Exists(Constants.DataLogPath))
            {
                FileInfo file = new(Constants.DataLogPath);
                file.Directory?.Create();
                File.WriteAllText(file.FullName, "");
            }

            lock (LogFileLock)
            {
                File.AppendAllLinesAsync(Constants.DataLogPath, new[]
                {
                    "#@#LOG ENTRY#@#" + GetTime() + " | " + logSeverityLevel + " | " + log1
                });
            }
        }
        catch (Exception e)
        {
            CriticalError(e, log);
        }
    }

    private static void CriticalError(Exception e, object? log)
    {
        try
        {
            Console.WriteLine("#############1#############");
            Console.WriteLine("CRITICAL ERROR IN LOGGER APPLICATION! NOTIFY ASAP!");
            Console.WriteLine(e);
            Console.WriteLine("#############2#############");
            if (log == null)
                Console.WriteLine("[null]");
            else
                Console.WriteLine(log);

            Console.WriteLine("#############3#############");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static string GetTime()
    {
        return DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    public static void LogQuery(string query, Dictionary<string, object?>? args)
    {
        if (args != null)
            foreach (var (key, value) in args)
                query = query.Replace(key, value?.ToString() ?? "NULL");

        WriteLine(query, LogSeverityLevel.DatabaseQuery);
    }
}