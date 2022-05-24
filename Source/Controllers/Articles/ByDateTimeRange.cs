#region includes

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticlesByDateTimeRange : ControllerBase
{
    [MapToApiVersion("1.0")]
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