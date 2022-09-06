#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using OfficeOpenXml;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Calendar")]
[Route("v{version:apiVersion}/calendar")]
[Route("/calendar")]
public class PopulateCalendarTable : ControllerBase
{
    /// <summary>
    ///     Adds date on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Days Added</response>
    /// <response code="500">Can't connect to server or Days not Added</response>
    [MapToApiVersion("1.0")]
    [HttpPut]
    public ObjectResult AddCalendarDb2(List<IFormFile> file, string year)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using var package = new ExcelPackage(file[0].OpenReadStream());
        var worksheet = package.Workbook.Worksheets[0];

        //print from cell A3 to AE33
        //date 1 august 2022
        var d = new DateTime(int.Parse(year), 8, 1);
        for (int i = 3; i <= 33; i++)
        {
            for (int j = 1; j <= 30; j++)
            {
                var cell = worksheet.Cells[i, j];
                if (cell.Value != null)
                {
                    var query = "INSERT IGNORE INTO Days VALUES ( '" + d.ToString("yyyy-MM-dd") + "' );";
                    var date = cell.Value.ToString();
                    var results = Database.Execute(query, GlobalVariables.DbConfigVar);

                    for(int k = j+1; worksheet.Cells[i,k]==null; k++){
                        var cell2 = worksheet.Cells[i, k];
                        //color of date2
                        var color2 = cell2.Style.Fill.BackgroundColor.Rgb;
                        switch(color2){
                            case"FFFF0000": //festività
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 1);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFC5DFB3"://esame di profitto
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 3);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFAC75D4"://laurea Magistrale
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 4);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case ""://lezione
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 5);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFD9D9D9"://sabato
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 6);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFFFFF00"://laureee di 1 livello
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 7);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFFFC000"://vacanze
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 8);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                            case "FFBCD5ED"://altre attività
                            {
                                query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 9);";
                                Database.Execute(query, GlobalVariables.DbConfigVar);
                                break;
                            }
                        }
                    }

                    //save  name of color of cell
                    var color = cell.Style.Fill.BackgroundColor.Rgb;
                    switch(color){
                        case"FFFF0000": //festività
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 1);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFC5DFB3"://esame di profitto
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 3);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFAC75D4"://laurea Magistrale
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 4);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case ""://lezione
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 5);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFD9D9D9"://sabato
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 6);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFFFFF00"://laureee di 1 livello
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 7);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFFFC000"://vacanze
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 8);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                        case "FFBCD5ED"://altre attività
                        {
                            query = "INSERT IGNORE INTO appartiene VALUES ('" + d.ToString("yyyy-MM-dd") + "' , 9);";
                            Database.Execute(query, GlobalVariables.DbConfigVar);
                            break;
                        }
                    }
                    //print color of cell
                    Console.WriteLine(date + " " +color);


                d = d.AddDays(1);
                }
                
            }
        }

        return Ok("OK");

        //Console.WriteLine(sb.ToString());
        //Console.ReadKey();
        
    }
}