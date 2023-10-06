#region

using System.Data;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Utils.Database;

public static class HandleDataUtil
{
    public static JArray GetResultsAsJArray(DataTable results)
    {
        var b = new JArray();
        foreach (DataRow row in results.Rows)
        {
            var c = new JObject();
            foreach (DataColumn column in results.Columns)
                c.Add(column.ColumnName, row[column].ToString());
            b.Add(c);
        }

        return b;
    }
}
