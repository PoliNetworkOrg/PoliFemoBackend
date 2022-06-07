#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticlesByStartingId : ControllerBase
{
    /// <summary>
    ///     Get articles by starting id
    /// </summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(10)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <param name="id"></param>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    // public ObjectResult SearchArticles(uint id)
    // {
    //     try
    //     {
    //         var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
    //         return articlesToSearchInto == null
    //             ? ResultUtil.ExceptionResult(exception)
    //             : Ok(articlesToSearchInto.FilterByStartingId(id));
    //     }
    //     catch (Exception ex)
    //     {
    //         return ResultUtil.ExceptionResult(ex);
    //     }
    // }
    public ObjectResult SearchArticlesDb(uint id)
    {
        var results = Database.ExecuteSelect(
            "SELECT * FROM Articles WHERE id_article >= @id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object>
            {
                { "@id", id }
            });

        return Ok(results);
    }
}