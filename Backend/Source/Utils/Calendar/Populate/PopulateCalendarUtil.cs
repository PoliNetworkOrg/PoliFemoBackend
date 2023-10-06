#region

using OfficeOpenXml;
using PoliFemoBackend.Source.Data;

#endregion

namespace PoliFemoBackend.Source.Utils.Calendar.Populate;

public static class PopulateCalendarUtil
{
    internal static void Populate(List<IFormFile> file, string year)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(file[0].OpenReadStream());
        var worksheet = package.Workbook.Worksheets[0];

        //date 1 august 2022
        var d = new DateTime(int.Parse(year), 8, 1);
        for (var j = 1; j <= 30; j++)
            for (var i = 3; i <= 33; i++)
            {
                var cell = worksheet.Cells[i, j];
                if (cell.Value == null)
                    continue;

                var query =
                    "INSERT IGNORE INTO Days VALUES ( '" + d.ToString("yyyy-MM-dd") + "' );";
                var date = cell.Value.ToString();
                var results = PoliNetwork.Db.Utils.Database.Execute(
                    query,
                    GlobalVariables.DbConfigVar
                );

                //save  name of color of cell
                var color = cell.Style.Fill.BackgroundColor.Rgb;

                ColorUtil.AddBelongsToBasedOnColor(color, d);

                //print color of cell
                Console.WriteLine(date + " " + color);

                if (worksheet.Cells[i, j].Merge)
                {
                    d = d.AddDays(1);
                    continue;
                }

                //var c =worksheet.Cells[i, j+1];
                for (var k = j + 1; worksheet.Cells[i, k].Value == null && k < 30; k++)
                {
                    var cell2 = worksheet.Cells[i, k];
                    //color of date2
                    var color2 = cell2.Style.Fill.BackgroundColor.Rgb;
                    ColorUtil.AddBelongsToBasedOnColor2(color2, d);
                }

                d = d.AddDays(1);
            }
    }
}
