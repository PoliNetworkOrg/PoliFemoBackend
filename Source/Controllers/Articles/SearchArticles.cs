using Microsoft.AspNetCore.Mvc;

namespace PoliFemoBackend.Source.Controllers.Article
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleByIdController : ControllerBase
    {

        [HttpGet]
        [HttpPost]
        public ObjectResult SearchArticles(int? id, string? author, DateTime? start, DateTime? end, Boolean getNextIds = false)
        {
            if (id != null) {
                if (!getNextIds) { //Search by id
                    try
                    {
                        var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                        return articlesToSearchInto == null
                            ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                            : Ok(Utils.ArticleUtil.FilterById(articlesToSearchInto, id.GetValueOrDefault()));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("no se pudo obtener los articulos");
                        return Utils.ArticleUtil.ErrorFindingArticles(ex);
                    }


                } else { //Search by starting id
                    try
                    {
                        var (articlesToSearchInto, exception) = Utils.ArticleUtil.GetArticles();
                        return articlesToSearchInto == null
                            ? Utils.ArticleUtil.ErrorFindingArticles(exception)
                            : Ok(Utils.ArticleUtil.FilterByStartingId(articlesToSearchInto, id.GetValueOrDefault()));
                    }
                    catch (Exception ex)
                    {
                        return Utils.ArticleUtil.ErrorFindingArticles(ex);
                    }
                }


            } else {
                if (author != null) { //Search by author
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
                


                if (start != null && end != null) { //Search by date range
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


                } else { //Get future articles
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
    }
}
