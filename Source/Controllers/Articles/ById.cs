#region

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[Route("/articles/byid")]
public class ArticleByIdController : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(uint id)
    {
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(ArticleUtil.FilterById(articlesToSearchInto, id));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}