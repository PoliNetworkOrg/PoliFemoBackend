#region

using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Calendar;

[ApiController]

[ApiExplorerSettings(GroupName = "Calendar")]

[Route("/calendar/exam")]
public class AddExam : ControllerBase
{
    /// <summary>
    ///     Adds exams on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Exams Added</response>
    /// <response code="500">Can't connect to server or Exams not Added</response>
    
    [HttpPut]
    public ObjectResult AddExamDb(List<IFormFile> file)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //number of worksheet
        using var package = new ExcelPackage(file[0].OpenReadStream());
        var n = package.Workbook.Worksheets.Count;

        for (var i = 0; i < n; i++)
        {
            var worksheet = package.Workbook.Worksheets[i];
            //number of rows
            var rows = worksheet.Dimension.Rows;
            //number of columns
            var columns = worksheet.Dimension.Columns;
            for (var j = 5; j <= rows; j++)
            {
                var query =
                    "INSERT IGNORE INTO Exams VALUES (null, '@cod_mat', \"@insegnamento\", '@sede', '@semestre', \"@docente\", '@orario' , '@giorno', \"@lista\" );";
                for (var k = 1; k <= columns; k++)
                {
                    var cell = worksheet.Cells[j, k];
                    if (cell.Value != null)
                        switch (k)
                        {
                            case 1:
                            {
                                query = query.Replace("@cod_mat", cell.Value.ToString());
                                break;
                            }
                            case 2:
                            {
                                query = query.Replace("@insegnamento", cell.Value.ToString());
                                break;
                            }
                            case 4:
                            {
                                query = query.Replace("@sede", cell.Value.ToString());
                                break;
                            }
                            case 5:
                            {
                                query = query.Replace("@semestre", cell.Value.ToString());
                                break;
                            }
                            case 6:
                            {
                                query = query.Replace("@docente", cell.Value.ToString());
                                break;
                            }
                            case 8:
                            {
                                //verify if date in null or not
                                var c = cell.Value.ToString();
                                if (c != null)
                                {
                                    var date = DateTime.Parse(c);
                                    //split datetime to time and date
                                    query = query.Replace("@orario", date.TimeOfDay.ToString("hh\\:mm\\:ss"));
                                    query = query.Replace("@giorno", date.Date.ToString("yyyy-MM-dd"));
                                }
                                else
                                {
                                    query = query.Replace("@orario", "null");
                                    query = query.Replace("@giorno", "null");
                                }

                                break;
                            }
                            case 9:
                            {
                                query = query.Replace("@lista", cell.Value.ToString());
                                break;
                            }
                        }
                }

                var results = Database.Execute(query, GlobalVariables.DbConfigVar);
            }
        }

        return Ok("Ok");
    }
}