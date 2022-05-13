using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers.Article
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleByIdController : ControllerBase
    {

        [HttpGet]
        [HttpPost]
        public ObjectResult SearchArticles(int id)
        {
            try
            {
                var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                    : Ok(Utils.ArticleUtil.FilterById(articlesToSearchInto, id));
            }
            catch (Exception ex)
            {
                return Utils.ArticleUtil.ErrorFindingArticles(ex);
            }
        }
    }
}
