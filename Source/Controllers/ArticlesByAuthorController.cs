#region

using Microsoft.AspNetCore.Mvc;

#endregion

namespace PoliFemoBackend.Source.Controllers;

[ApiController]
[Route("[controller]")]
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
                ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                : Ok(Utils.ArticleUtil.FilterByAuthor(articlesToSearchInto, author));
        }
        catch (Exception ex)
        {
            return Utils.ArticleUtil.ErrorFindingArticles(ex);
        }
    }


}