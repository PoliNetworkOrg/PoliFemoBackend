#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[Route("/articles/targetingthefuture")]
public class ArticlesTargetingTheFuture : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles()
    {
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(ArticleUtil.FilterByTargetingTheFuture(articlesToSearchInto));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}