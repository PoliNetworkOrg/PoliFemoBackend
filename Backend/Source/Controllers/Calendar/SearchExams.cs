#region

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Calendar;

[ApiController]
[ApiExplorerSettings(GroupName = "Exam")]
[Route("/exam/search")]
public class SearchExam : ControllerBase
{
    /// <summary>
    ///     Searches for available groups
    /// </summary>
    /// <param name="cod_mat" example="056951">Course code</param>
    /// <param name="insegnamento" example="ALGEBRA LINEARE E GEOMETRIA">Course name</param>
    /// <param name="sede" example="MI">Possible values: MI, BV</param>
    /// <param name="semestre" example="2">Possible values: 1, 2</param>
    /// <param name="docente" example="LELLA PAOLO">Teacher name</param>
    /// <param name="orario" example="15:00:00">Time</param>
    /// <param name="giorno" example="17-06-2022">Date</param>
    /// <param name="lista" example="365 - ingegneria matematica">Part of Cds name</param>
    /// <returns>List of exams</returns>
    /// <response code="200">Returns the array of exams</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available exam</response>
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public ActionResult SearchExamDb(string? cod_mat, string? insegnamento, string? sede, int? semestre,
        string? docente, string? orario, string? giorno, string? lista)
    {
        var d = new Dictionary<string, object?> { { "@insegnamento", insegnamento } };
        var query =
            "SELECT cod_mat, insegnamento, sede, docente, orario, giorno FROM Exam WHERE  insegnamento LIKE '%@insegnamento%'";
        if (cod_mat != null)
        {
            query += " AND cod_mat = '@cod_mat'";
            d.Add("@cod_mat", cod_mat);
        }

        if (sede != null)
        {
            query += " AND sede = '@sede'";
            d.Add("@sede", sede);
        }

        if (semestre != null)
        {
            query += " AND semestre = '@semestre'";
            d.Add("@semestre", semestre);
        }

        if (docente != null)
        {
            query += "  docente LIKE '%@docente%'";
            d.Add("@docente", docente);
        }

        if (orario != null)
        {
            query += " AND orario = '@orario'";
            d.Add("@orario", orario);
        }

        if (giorno != null)
        {
            query += " AND giorno = '@giorno'";
            d.Add("@giorno", giorno);
        }

        if (lista != null)
        {
            query += " AND lista LIKE '%@lista%';";
            d.Add("@lista", lista);
        }

        var results = Database.ExecuteSelect(query, GlobalVariables.DbConfigVar, d);

        if (results == null) return StatusCode(500);
        if (results.Rows.Count == 0) return NoContent();


        if (results.Rows.Count == 0) return NoContent();

        var sg = SampleNuGet.Utils.SerializeUtil.JsonToString(results);
        HttpContext.Response.ContentType = "application/json";

        var ag = JsonConvert.DeserializeObject(sg ?? "") as JArray;

        var o = new JObject { { "groups", ag } };
        return Ok(o);
    }
}