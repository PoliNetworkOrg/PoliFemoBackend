#region

using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using Path = System.IO.Path;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class AddCalendarControllers : ControllerBase
{
    /// <summary>
    ///     Add groups on Database
    /// </summary>
    /// <returns>Nothing</returns>
    /// <response code="200">Days Added</response>
    /// <response code="500">Can't connect to server or Days not Added</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult AddCalendarDb(List<IFormFile> file, string year)
    {
        var i = 0;
        string query;
        int results;
        //populate the database with day of year
        var d = new DateTime(int.Parse(year), 8, 1);
        // if (GlobalVariables.DbConfigVar == null) return Ok("error");

        for (i = 1; i <= 365; i++)
        {
            query = "INSERT IGNORE INTO Days VALUES ( '" + d.ToString("yyyy-MM-dd") + "' );";

            results = Database.Execute(query, GlobalVariables.DbConfigVar);
            switch (d.DayOfWeek)
            {
                //se d è sabato
                case DayOfWeek.Saturday:
                    query = "INSERT IGNORE INTO appartiene VALUES (" + results + " , 6);";
                    Database.Execute(query, GlobalVariables.DbConfigVar);
                    break;
                //se d è domenica
                case DayOfWeek.Sunday:
                    query = "INSERT IGNORE INTO appartiene VALUES (" + results + " , 1);";
                    Database.Execute(query, GlobalVariables.DbConfigVar);
                    break;
                case DayOfWeek.Monday:

                case DayOfWeek.Tuesday:

                case DayOfWeek.Wednesday:

                case DayOfWeek.Thursday:

                case DayOfWeek.Friday:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(year));
            }


            //add day
            d = d.AddDays(1);
        }

        var sb = new StringBuilder();
        //get name path file
        var path = Path.GetTempFileName();
        using var reader = new PdfReader(file[0].OpenReadStream());

        //read the text from each page
        sb.Append(PdfTextExtractor.GetTextFromPage(reader, 2));


        //ESAMI DI PROFITTO
        var line = sb.ToString().Split('\n').First(x => x.Contains("SESSIONE PER GLI ESAMI"));
        i = 0;
        while (i < 4)
        {
            //VAI ALLA RIGA DOPO
            line = sb.ToString().Split('\n').SkipWhile(x => x != line).Skip(1).First();
            //extract the date
            var date1 = line.Split("dal").Last().Split("al").First().Trim();
            var date2 = line.Split(" al ").Last().Split(" ").First().Trim();

            var d5 = DateTime.Parse(date1);
            var d6 = DateTime.Parse(date2);
            while (d5 <= d6)
            {
                //var query = "SELECT * INTO appartiene WHERE giorno = '" + d1.ToString("yyyy-MM-dd") + "' AND tipologia = 3;";
                //var results = Database.Execute(query, GlobalVariables.DbConfigVar);
                //if(results)
                var query6 = "INSERT IGNORE INTO appartiene VALUES ( '" + d5.ToString("yyyy-MM-dd") + "', 3 );";

                var results6 = Database.Execute(query6, GlobalVariables.DbConfigVar);
                d5 = d5.AddDays(1);
            }

            i++;
        }
        //Console.WriteLine(date1);
        //Console.WriteLine(date2);

        //PER LE LAUREE
        Console.WriteLine(line);
        //prendi la prima riga delle sessioni di laurea
        var array = sb.ToString().Split("SESSIONI DI LAUREA").Last().Split("\n");
        Console.WriteLine(line);
        //loop array
        var d3 = DateTime.Parse("2020-08-01");
        foreach (var item in array)
        {
            //se la riga contiene una parola
            if (!item.Contains("Laurea Triennale") && !item.Contains("Laurea Magistrale"))
                continue;

            //extract the date
            var date1 = item.Split("Laurea").First().Split("ì").Last().Trim();
            //Console.WriteLine(date1);
            if (date1 != "") d3 = DateTime.Parse(date1);

            var query4 = "";
            if (item.Contains("Laurea Triennale"))
                query4 = "INSERT IGNORE INTO appartiene VALUES ( '" + d3.ToString("yyyy-MM-dd") + "', 7 );";
            else
                query4 = "INSERT IGNORE INTO appartiene VALUES ( '" + d3.ToString("yyyy-MM-dd") + "', 4);";

            var results4 = Database.Execute(query4, GlobalVariables.DbConfigVar);
        }

        //PER LE LEZIONI
        line = sb.ToString().Split('\n').First(x => x.Contains("PERIODI DI LEZIONE"));
        line = sb.ToString().Split('\n').SkipWhile(x => x != line).Skip(2).First();
        i = 0;
        while (i < 2)
        {
            var date1 = line.Split("Dal").Last().Split("al").First().Trim();
            var date2 = line.Split(" al ").Last().Split(",").First().Trim();
            var d4 = DateTime.Parse(date1);
            var d5 = DateTime.Parse(date2);
            while (d4 <= d5)
            {
                if (d4.DayOfWeek != DayOfWeek.Sunday)
                {
                    var query3 = "INSERT IGNORE INTO appartiene VALUES ( '" + d4.ToString("yyyy-MM-dd") + "', 5 );";
                    var results3 = Database.Execute(query3, GlobalVariables.DbConfigVar);
                }

                d4 = d4.AddDays(1);
            }

            i++;
            line = sb.ToString().Split('\n').SkipWhile(x => x != line).Skip(4).First();
        }

        //PER LE VACANZE

        var d1 = DateTime.Parse("2022-08-01");
        var d2 = DateTime.Parse("2022-08-24");

        while (d1 <= d2)
        {
            if (d1.DayOfWeek != DayOfWeek.Sunday)
            {
                var query3 = "INSERT IGNORE INTO appartiene VALUES ( '" + d1.ToString("yyyy-MM-dd") + "', 8 );";
                var results3 = Database.Execute(query3, GlobalVariables.DbConfigVar);
            }

            d1 = d1.AddDays(1);
        }

        query = "INSERT IGNORE INTO appartiene VALUES ( '2022-10-31', 8 );";
        results = Database.Execute(query, GlobalVariables.DbConfigVar);
        query = "INSERT IGNORE INTO appartiene VALUES ( '2022-12-9', 8 );";
        results = Database.Execute(query, GlobalVariables.DbConfigVar);
        query = "INSERT IGNORE INTO appartiene VALUES ( '2022-12-10', 8 );";
        results = Database.Execute(query, GlobalVariables.DbConfigVar);

        d1 = DateTime.Parse("2022-12-24");
        d2 = DateTime.Parse("2023-01-07");

        while (d1 <= d2)
        {
            if (d1.DayOfWeek != DayOfWeek.Sunday)
            {
                var query3 = "INSERT IGNORE INTO appartiene VALUES ( '" + d1.ToString("yyyy-MM-dd") + "', 8 );";
                var results3 = Database.Execute(query3, GlobalVariables.DbConfigVar);
            }

            d1 = d1.AddDays(1);
        }


        d1 = DateTime.Parse("2023-04-07");
        d2 = DateTime.Parse("2023-04-11");

        while (d1 <= d2)
        {
            if (d1.DayOfWeek != DayOfWeek.Sunday)
            {
                var query3 = "INSERT IGNORE INTO appartiene VALUES ( '" + d1.ToString("yyyy-MM-dd") + "', 8 );";
                var results3 = Database.Execute(query3, GlobalVariables.DbConfigVar);
            }

            d1 = d1.AddDays(1);
        }

        const string query2 = "INSERT IGNORE INTO appartiene VALUES ( '2023-04-24', 8 );";
        var results2 = Database.Execute(query2, GlobalVariables.DbConfigVar);

        d1 = DateTime.Parse("2023-07-29");
        d2 = DateTime.Parse("2023-08-24");

        while (d1 <= d2)
        {
            if (d1.DayOfWeek != DayOfWeek.Sunday)
            {
                var query3 = "INSERT IGNORE INTO appartiene VALUES ( '" + d1.ToString("yyyy-MM-dd") + "', 8 );";
                var results3 = Database.Execute(query3, GlobalVariables.DbConfigVar);
            }

            d1 = d1.AddDays(1);
        }


        return Ok("OK");

        //Console.WriteLine(sb.ToString());
        //Console.ReadKey();
    }
}