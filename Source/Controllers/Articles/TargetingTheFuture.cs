#region includes

using Microsoft.AspNetCore.Mvc;
using PoliFemoBackend.Source.Utils;
using PoliNetworkBot_CSharp.Code.Data;

#endregion

namespace PoliFemoBackend.Source.Controllers.Articles;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Route("[controller]")]
public class ArticlesTargetingTheFuture : ControllerBase
{
    [MapToApiVersion("1.0")]
    [HttpGet]
    [HttpPost]
    public ObjectResult SearchArticles()
    {
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

    //date today    
    public ObjectResult SearchArticlesByTargetingTheFuture()
    {
        var dateToday = DateTime.Now.ToString("yyyy-MM-dd");
        var results = Utils.Database.ExecuteSelect(
            "SELECT * FROM article WHERE publishTime > @dateToday",
            GlobalVariables.DbConfigVar);

        return Ok(results);
    }
}