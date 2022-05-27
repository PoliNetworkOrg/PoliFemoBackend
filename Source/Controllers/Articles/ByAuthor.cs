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
            "SELECT article.id_article, article.title, article.subtitle, article.text_, article.publishTime, article.targetTime, article.music, article.id_media " +
            "FROM author, article, scritto " +
            " WHERE scritto.id_article = article.id_article AND scritto.id_author = author.id_author AND author.name = @author",
            GlobalVariables.DbConfigVar,
            new Dictionary<string, object>
            {
                {"@author", author}
            });

        return Ok(results);
    }




}