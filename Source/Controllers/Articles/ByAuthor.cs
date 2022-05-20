#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Controllers.Article;

[ApiController]
[Route("/articles/byauthor")]
public class ArticlesByAuthorController : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(string author)
    {
        try
        {
            var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? Utils.ResultUtil.ExceptionResult(exception)
                : Ok(Utils.ArticleUtil.FilterByAuthor(articlesToSearchInto, author));
        }
        catch (Exception ex)
        {
            return Utils.ResultUtil.ExceptionResult(ex);
        }
    }


}