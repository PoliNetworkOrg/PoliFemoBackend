#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[Route("/articles/byauthor")]
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
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(string author)
    {
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(articlesToSearchInto.FilterByAuthor(author));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}