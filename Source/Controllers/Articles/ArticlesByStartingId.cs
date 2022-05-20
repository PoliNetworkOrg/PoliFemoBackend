#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[Route("/articles/bystartingid")]
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
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(uint id)
    {
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(articlesToSearchInto.FilterByStartingId(id));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}