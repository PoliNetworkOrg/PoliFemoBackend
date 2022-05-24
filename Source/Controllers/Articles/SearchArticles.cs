#region includes

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class SearchArticlesController : ControllerBase
{
    /// <summary>
    ///     Searches for articles with the specified parameters
    /// </summary>
    /// <param name="id" example="MIA">The article ID</param>
    /// <param name="author" example="Mario Rossi">The article author</param>
    /// <param name="start" example="2022-05-18T12:15:00Z">Article publish time</param>
    /// <param name="end" example="2022-05-18T14:15:00Z">Article target time</param>
    /// <param name="getNextIds" example="true">
    ///     If true, returns the articles with ID greater than the one provided in the ID
    ///     field
    /// </param>
    /// <returns>An array of articles</returns>
    /// <response code="200">Returns the array of articles</response>
    /// <response code="500">Can't fetch the articles</response>
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles(uint? id, string? author, DateTime? start, DateTime? end,
        bool getNextIds = false)
    {
        if (id != null)
        {
            if (!getNextIds) //Search by id
            {
                try
                {
                    var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
                    return articlesToSearchInto == null
                        ? ResultUtil.ExceptionResult(exception)
                        : Ok(articlesToSearchInto.GetArticleById(id.GetValueOrDefault()));
                }
                catch (Exception ex)
                {
                    return ResultUtil.ExceptionResult(ex);
                }
            }

            //Search by starting id
            try
            {
                var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? ResultUtil.ExceptionResult(exception)
                    : Ok(articlesToSearchInto.FilterByStartingId(id.GetValueOrDefault()));
            }
            catch (Exception ex)
            {
                return ResultUtil.ExceptionResult(ex);
            }
        }

        if (author != null) //Search by author
        {
            try
            {
                var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
                return articlesToSearchInto == null
                    ? ResultUtil.ExceptionResult(exception)
                    : Ok(articlesToSearchInto.FilterByAuthor(author));
            }
            catch (Exception ex)
            {
                return ResultUtil.ExceptionResult(ex);
            }
        }

        if (start != null && end != null) //Search by date range
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

        //Get future articles
        try
        {
            var (articlesToSearchInto, exception) = ArticleUtil.GetArticles();
            return articlesToSearchInto == null
                ? ResultUtil.ExceptionResult(exception)
                : Ok(ArticleUtil.FilterByTargetingTheFuture(articlesToSearchInto));
        }
        catch (Exception ex)
        {
            return ResultUtil.ExceptionResult(ex);
        }
    }
}