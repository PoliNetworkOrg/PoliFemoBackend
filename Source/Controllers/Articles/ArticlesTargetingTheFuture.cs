using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers.Article
{
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
                var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? Utils.ResultUtil.ExceptionResult(exception)
                    : Ok(Utils.ArticleUtil.FilterByTargetingTheFuture(articlesToSearchInto));
            }
            catch (Exception ex)
            {
                return Utils.ResultUtil.ExceptionResult(ex);
            }
        }
    }
}
