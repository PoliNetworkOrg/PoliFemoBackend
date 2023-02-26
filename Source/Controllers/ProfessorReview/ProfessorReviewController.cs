using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Controllers.ProfessorReview;

[ApiController]
[ApiExplorerSettings(GroupName = "Professor Review")]
[Route("/professor_review")]
public class ProfessorReviewController : ControllerBase
{
    // https://www4.ceda.polimi.it/manifesti/manifesti/controller/ricerche/RicercaPerDocentiPublic.do?evn_didattica=evento&k_doc=XXXXX


    /*
     * TABLE STRUCTURE
     * professor_reviews (
     *      professor_id, [UINT]
     *      user_id, [same type as users.user_id table.column]
     *      review, [DECIMAL]
     *      review_category [UINT]
     * )
     * review_categories (
     *      id, [UINT]
     *      name [STRING(100)]
     * )
     * link between the two tables: professor_reviews.review_category = review_categories.id
     *
     * primary keys:
     * professor_reviews (professor_id,user_id,review_category)
     * review_categories (id)
     */

    [HttpGet]
    public ActionResult GetProfessorReview(uint professorId)
    {
        var q =
            "SELECT review_categories.name, AVG(professor_reviews.review)  FROM professor_reviews, review_categories  " +
            "WHERE professor_reviews.review_category = review_categories.id " +
            $"AND professor_id = {professorId} " +
            "GROUP BY professor_reviews.review_category ";

        var dt = Database.ExecuteSelect(q, GlobalVariables.DbConfigVar);
        var jArray = new JArray();
        if (dt == null)
            return Ok(jArray); //todo: mettere 404?

        foreach (DataRow dr in dt.Rows)
        {
            var jObject = new JObject
            {
                ["name"] = dr.ItemArray[0]?.ToString(),
                ["review"] = Convert.ToDecimal(dr.ItemArray[1])
            };

            jArray.Add(jObject);
        }

        return Ok(jArray);
    }

    [HttpPost]
    [Authorize]
    public ActionResult PostProfessorReview(uint professorId, uint categoryReview, decimal reviewValue)
    {
        if (reviewValue is < 1 or > 5)
            return StatusCode(500, ""); //todo: controllare questo

        var sub = AuthUtil.GetSubjectFromHttpRequest(Request);

        const string query = "INSERT IGNORE INTO professor_reviews " +
                             "(user_id, review_category, professor_id, review) " +
                             "VALUES " +
                             "(SHA2(@sub, 256), @review_category, @professor_id, @review)";
        var parameters = new Dictionary<string, object?>
        {
            { "@sub", sub },
            { "@review_category", categoryReview },
            { "@professor_id", professorId },
            { "@review", reviewValue }
        };

        var r = Database.Execute(query, GlobalVariables.DbConfigVar, parameters);
        return Ok(r);
    }

    [HttpGet]
    [Route("/categories")]
    public ActionResult GetCategories()
    {
        const string q = "SELECT id, name  FROM review_categories  ";

        var dt = Database.ExecuteSelect(q, GlobalVariables.DbConfigVar);
        var jArray = new JArray();
        if (dt == null)
            return Ok(jArray); //todo: mettere 404?

        foreach (DataRow dr in dt.Rows)
        {
            var jObject = new JObject
            {
                ["id"] = Convert.ToUInt32(dr.ItemArray[0]),
                ["name"] = dr.ItemArray[1]?.ToString()
            };

            jArray.Add(jObject);
        }

        return Ok(jArray);
    }
}