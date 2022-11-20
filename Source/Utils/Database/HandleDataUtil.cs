using System.Data;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Utils.Database;

public static class HandleDataUtil
{
    public static JArray GetResultsAsJArray(DataTable results)
    {
        var listDataColumns = GetDataColumnList(results.Columns);
        var listDataRows = GetDataRowList(results.Rows);
        var b = new JArray();
        foreach (DataRow row in results.Rows)
        {
            var c = new JObject();
            foreach (DataColumn column in results.Columns) c.Add(column.ColumnName, row[column].ToString());
            b.Add(c);
        }

        return b;
    }

    private static List<DataRow> GetDataRowList(DataRowCollection dataRowCollection)
    {
        var rows = new List<DataRow>();
        foreach (var row in dataRowCollection)
            if (row is DataRow drDataRow)
                rows.Add(drDataRow);

        return rows;
    }

    private static JObject GetRowAsJObject(DataRow row, List<DataColumn> listDataColumns)
    {
        var c = new JObject();
        listDataColumns.ForEach(x => c.Add(x.ColumnName, row[x].ToString()));
        return c;
    }

    private static List<DataColumn> GetDataColumnList(DataColumnCollection resultsColumns)
    {
        var r = new List<DataColumn>();
        foreach (var col in resultsColumns)
            if (col is DataColumn dataColumn)
                r.Add(dataColumn);

        return r;
    }
}