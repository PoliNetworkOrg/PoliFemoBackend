#region includes

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticleByIdController : ControllerBase
{
    // public ObjectResult SearchArticles(uint id)
    // {
    //     try
    //     {
    //         var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
    //         return articlesToSearchInto == null
    //             ? ResultUtil.ExceptionResult(exception)
    //             : Ok(articlesToSearchInto.GetArticleById(id));
    //     }
    //     catch (Exception ex)
    //     {
    //         return ResultUtil.ExceptionResult(ex);
    //     }
    // }
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticlesById(uint id)
    {
        var results = Database.ExecuteSelect(
            "SELECT * FROM article WHERE id_article = @id",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object>
            {
                {"@id", id}
            });

        return Ok(results);
    }
}