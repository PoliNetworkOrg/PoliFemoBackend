using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesTargetingTheFutureController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public ObjectResult SearchArticles()
        {
            try
            {
                var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                    : Ok(Utils.ArticleUtil.FilterByTargetingTheFuture(articlesToSearchInto));
            }
            catch (Exception ex)
            {
                return Utils.ArticleUtil.ErrorFindingArticles(ex);
            }
        }
    }
}