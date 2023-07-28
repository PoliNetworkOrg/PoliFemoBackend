#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliNetwork.Db.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Calendar;

[ApiController]
[ApiExplorerSettings(GroupName = "Calendar")]
[Route("/calendar/modify")]
public class ModifyDateControllers : ControllerBase
{
    /// <summary>
    ///     Modifies date on Database
    /// </summary>
    /// <param name="date" example="2022-08-01">Date</param>
    /// <param name="tipologia_new" example="1">
    ///     Possible values: 1=Festivo, 2=Esame, 3=Esame di Profitto, 4=Lauree Magistrali,
    ///     5=Lezione, 6= Sabato, 7=Lauree 1 liv, 8=Vacanza, 9=altre attività
    /// </param>
    /// <param name="tipologia_old" example="1">
    ///     Possible values: 1=Festivo, 2=Esame, 3=Esame di Profitto, 4=Lauree Magistrali,
    ///     5=Lezione, 6= Sabato, 7=Lauree 1 liv, 8=Vacanza, 9=altre attività
    /// </param>
    /// <returns>Nothing</returns>
    /// <response code="200">Date modified</response>
    /// <response code="500">Can't connect to server or Date not modified</response>
    [HttpPost]
    public ObjectResult ModifiedTypeDateDb(DateTime date, int tipologia_old, int tipologia_new)
    {
        var query = "UPDATE belongsTo SET id_tipologia = " + tipologia_new + " WHERE id_giorno = '" +
                    date.ToString("yyyy-MM-dd") + "' AND id_tipologia = " + tipologia_old + " ;";
        int? results = Database.Execute(query, GlobalVariables.DbConfigVar);


        return results switch
        {
            null => Ok("error"),
            <= 0 => Ok("no effect"),
            _ => Ok("done")
        };
    }


    /// <summary>
    ///     Adds date on Database
    /// </summary>
    /// <param name="date" example="2022-08-01">Date</param>
    /// <param name="tipologia" example="1">
    ///     Possible values: 1=Festivo, 2=Esame, 3=Esame di Profitto, 4=Lauree Magistrali,
    ///     5=Lezione, 6= Sabato, 7=Lauree 1 liv, 8=Vacanza, 9=altre attività
    /// </param>
    /// <returns>Nothing</returns>
    /// <response code="200">Date added</response>
    /// <response code="500">Can't connect to server or Date not added</response>
    [HttpPut]
    public ObjectResult AddTypeDateDb(DateTime date, int tipologia)
    {
        var query = "INSERT IGNORE INTO belongsTo VALUES ('" + date.ToString("yyyy-MM-dd") + "', " + tipologia + " );";
        int? results = Database.Execute(query, GlobalVariables.DbConfigVar);

        return results switch
        {
            null => Ok("error"),
            <= 0 => Ok("no effect"),
            _ => Ok("done")
        };
    }

    /// <summary>
    ///     Deletes date from Database
    /// </summary>
    /// <param name="date" example="2022-08-01">Date</param>
    /// <param name="tipologia" example="1">
    ///     Possible values: 1=Festivo, 2=Esame, 3=Esame di Profitto, 4=Lauree Magistrali,
    ///     5=Lezione, 6= Sabato, 7=Lauree 1 liv, 8=Vacanza, 9=altre attività
    /// </param>
    /// <returns>Nothing</returns>
    /// <response code="200">Date removed</response>
    /// <response code="500">Can't connect to server or Date not removed</response>
    [HttpDelete]
    public ObjectResult RemoveTypeDateDb(DateTime date, int tipologia)
    {
        var query = "DELETE FROM belongsTo WHERE id_tipologia = " + tipologia + " AND id_giorno = '" +
                    date.ToString("yyyy-MM-dd") + "';";
        int? results = Database.Execute(query, GlobalVariables.DbConfigVar);

        return results switch
        {
            null => Ok("error"),
            <= 0 => Ok("no effect"),
            _ => Ok("done")
        };
    }
}