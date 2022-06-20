#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Admin;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ModifyDateControllers : ControllerBase
{
    /// <summary>
    ///     Add groups on Database
    /// </summary>
    /// <param name="date" example="2022-08-01">Date</param>
    /// <param name="operazione" example="rimuovi">Possible values: rimuovi, aggiungi, modifica</param>
    /// <param name="tipologia" example="1">
    ///     Possible values: 1=Festivo, 2=Esame, 3=Esame di Profitto, 4=Lauree Magistrali,
    ///     5=Lezione, 6= Sabato, 7=Lauree 1 liv, 8=Vacanza, 9=altre attività
    /// </param>
    /// <returns>Nothing</returns>
    /// <response code="200">Group Added</response>
    /// <response code="500">Can't connect to server or Group not Added</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult ModifiedTypeDateDb(DateTime date, int tipologia, string operazione)
    {
        int? results = null;
        if (GlobalVariables.DbConfigVar == null)
            return results switch
            {
                null => Ok("error"),
                <= 0 => Ok("no effect"),
                _ => Ok("done")
            };

        switch (operazione)
        {
            case "rimuovi":
            {
                var query = "DELETE FROM appartiene WHERE id_tipologia = " + tipologia + " AND id_giorno = '" +
                            date.ToString("yyyy-MM-dd") + "';";

                results = Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "aggiungi":
            {
                var query = "INSERT IGNORE INTO appartiene VALUES ('" + date.ToString("yyyy-MM-dd") + "', " +
                            tipologia + " );";
                results = Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
            case "modifica":
            {
                var query = "UPDATE appartiene SET id_tipologia = " + tipologia + " WHERE id_giorno = '" +
                            date.ToString("yyyy-MM-dd") + "';";
                results = Database.Execute(query, GlobalVariables.DbConfigVar);
                break;
            }
        }

        return results switch
        {
            null => Ok("error"),
            <= 0 => Ok("no effect"),
            _ => Ok("done")
        };
    }
}