#region

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[Route("/groups/search")]
public class SearchController : ControllerBase
{
    /// <summary>
    ///     Searches for available groups
    /// </summary>
    /// <param name="name" example="Informatica">Group name</param>
    /// <param name="year" example="2022">Year</param>
    /// <param name="degree" example="LT">Possible values: LT, LM, LU </param>
    /// <param name="type" example="C">Possible values: S, C, E</param>
    /// <param name="platform" example="TG">Possible values: WA, TG, FB</param>
    /// <param name="language" example="ITA">Possible values: ITA, ENG</param>
    /// <param name="office" example="Leonardo">Possible values: Bovisa, Como, Cremona, Lecco, Leonardo</param>
    /// <returns>An array of free groups</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [HttpGet]
    [HttpPost]
    public async Task<ObjectResult> SearchGroup([BindRequired] string name, string? year, string? degree, string? type,
        string? platform, string? language, string? office)
    {
        var json = await GroupsUtil.GetGroups();
        if (json == null)
            return GroupsUtil.ErrorInRetrievingGroups();

        //filtra per i parametri
        bool Filter(dynamic item) => item["class"].ToString().ToLower().Contains(name.ToLower()) &&
                                     (string.IsNullOrEmpty(year) || item.year.ToString().ToLower().Contains(year.ToLower())) &&
                                     (string.IsNullOrEmpty(type) || item.type.ToString().ToLower().Contains(type.ToLower())) &&
                                     (string.IsNullOrEmpty(degree) || item.degree.ToString().ToLower().Contains(degree.ToLower())) &&
                                     (string.IsNullOrEmpty(platform) || item.platform.ToString().ToLower().Contains(platform.ToLower())) &&
                                     (string.IsNullOrEmpty(language) || item.language.ToString().ToLower().Contains(language.ToLower())) &&
                                     (string.IsNullOrEmpty(office) || item.office.ToString().ToLower().Contains(office.ToLower()));

        var filtered =  GroupsUtil.Filter(json, (Func<dynamic, bool>)Filter);
        return GroupsUtil.ResultSearch(this, filtered);
    }
}