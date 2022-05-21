#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[Route("/articles/byDateTimeRange")]
public class ArticlesByDateTimeRange : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(DateTime? start, DateTime? end)
    {
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(ArticleUtil.FilterByDateTimeRange(articlesToSearchInto, start, end));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}