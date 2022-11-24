using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Utils;

public static class DateTimeUtil
{
    public static DateTime? ConvertToDateTime(string? s)
    {
        if (string.IsNullOrEmpty(s)) return null;

        try
        {
            return DateTime.Parse(s);
        }
        catch
        {
            // ignored
        }

        //2022-01-01T23:59:59.999

        try
        {
            var dates = s.Split('T');
            var yearMonthDay = dates[0].Split('-');
            var time = dates[1].Split('.');
            var hourMinuteSecond = time[0].Split(':');
            return new DateTime(
                Convert.ToInt32(yearMonthDay[0]),
                Convert.ToInt32(yearMonthDay[1]),
                Convert.ToInt32(yearMonthDay[1]),
                Convert.ToInt32(hourMinuteSecond[0]),
                Convert.ToInt32(hourMinuteSecond[1]),
                Convert.ToInt32(hourMinuteSecond[2]),
                Convert.ToInt32(time[1])
            );
        }
        catch
        {
            return null;
        }
    }

    public static string? ConvertToMySqlString(DateTime? dateTime)
    {
        return dateTime?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }
}