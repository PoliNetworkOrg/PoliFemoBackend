using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers.Article
{
    [ApiController]
    [Route("[controller]")]
    public class ArticlesByDateTimeRange : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        public ObjectResult SearchArticles(DateTime? start, DateTime? end)
        {
            try
            {
                var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                    : Ok(Utils.ArticleUtil.FilterByDateTimeRange(articlesToSearchInto, start, end));
            }
            catch (Exception ex)
            {
                return Utils.ArticleUtil.ErrorFindingArticles(ex);
            }
        }
    }
}