using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesByStartingIdController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public ObjectResult SearchArticles(string id)
        {
            try
            {
                var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                    : Ok(Utils.ArticleUtil.FilterByStartingId(articlesToSearchInto, id));
            }
            catch (Exception ex)
            {
                return Utils.ArticleUtil.ErrorFindingArticles(ex);
            }
        }
    }

}