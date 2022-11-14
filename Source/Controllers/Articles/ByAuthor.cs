#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticlesByAuthorController : ControllerBase
{
    /// <summary>
    ///     Get all articles by author
    /// </summary>
    /// <complexity>
    ///     <best>O(1)</best>
    ///     <average>O(1)</average>
    ///     <worst>O(n)</worst>
    /// </complexity>
    /// <param name="author"></param>
    /// <returns></returns>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    // public ObjectResult SearchArticles(string author)
    // {
    //     try
    //     {
    //         var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
    //         return articlesToSearchInto == null
    //             ? ResultUtil.ExceptionResult(exception)
    //             : Ok(articlesToSearchInto.FilterByAuthor(author));
    //     }
    //     catch (Exception ex)
    //     {
    //         return ResultUtil.ExceptionResult(ex);
    //     }
    // }
    public ObjectResult SearchArticlesDb(string author)
    {
        var results = Database.ExecuteSelect(
            "SELECT Articles.id_article, Articles.title, Articles.subtitle, Articles.text_, Articles.publishTime, Articles.targetTime, Articles.music, Articles.id_media " +
            "FROM Authors, Articles, scritto " +
            " WHERE scritto.id_article = Articles.id_article AND scritto.id_author = Authors.id_author AND Authors.name = @author",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object?>
            {
                { "@author", author }
            });

        return Ok(results);
    }
}