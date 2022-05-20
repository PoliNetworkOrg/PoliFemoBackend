#region

using System.Net;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils;

#endregion

namespace PoliFemoBackend.Source.Controllers.Groups;

[ApiController]
[Route("/groups/byname")]
public class GroupsByName : ControllerBase
{
    /// <summary>
    ///     Checks for available groups
    /// </summary>
    /// <param name="name" example="Informatica">Group name</param>
    /// <returns>An array of free groups</returns>
    /// <response code="200">Returns the array of groups</response>
    /// <response code="500">Can't connect to server</response>
    /// <response code="204">No available groups</response>
    [HttpGet]
    public async Task<ObjectResult> SearchGroupByName([BindRequired] string name)
    {
        //get content from url
        var content =
            await HtmlUtil.DownloadHtmlAsync(
                "https://raw.githubusercontent.com/PoliNetworkOrg/polinetworkWebsiteData/main/groups.json");

        var doc = new HtmlDocument();

        var c = content.GetData();
        if (c == null)
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" })
                { StatusCode = (int)HttpStatusCode.InternalServerError };


        var c1 = c.Replace("<", "&lt;");
        doc.LoadHtml(c1);


        //WriteLine doc
        //Console.WriteLine(doc.DocumentNode.InnerHtml);  //tenere non cancellare


        //convert doc to json
        var json = JsonConvert.DeserializeObject<dynamic>(doc.DocumentNode.InnerHtml);
        if (json == null)
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" })
                { StatusCode = (int)HttpStatusCode.InternalServerError };

        //print json file
        //return Ok(json);

        //crea  json vuoto
        var results = new JObject();

        //crea lista results dentro json

        var resultsList = new JArray();


        //cicla json

        foreach (var item in json.index_data)
            //se il nome del gruppo è uguale a quello passato come parametro
            if (item["class"].ToString().ToLower().Contains(name.ToLower()))
                //aggiungi risultato alla lista
                resultsList.Add(JObject.Parse(HttpUtility.HtmlDecode(item.ToString())));


        results["groups"] = resultsList;

        //se la lista è vuota
        if (results.Count == 0)
            return new ObjectResult(new { error = "Nessun gruppo trovato" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

        //se la lista contiene almeno un elemento
        return Ok(results);
    }
}