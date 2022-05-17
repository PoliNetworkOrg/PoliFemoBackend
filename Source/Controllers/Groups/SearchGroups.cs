using System.Net;
using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils;
using JsonConvert = Newtonsoft.Json.JsonConvert;
using JObject = Newtonsoft.Json.Linq.JObject;
using JArray = Newtonsoft.Json.Linq.JArray;
using System.Web;
using JSConverter = Newtonsoft.Json.JsonConverter; 




namespace PoliFemoBackend.Source.Controllers.Rooms;

[ApiController]
[Route("[controller]")]
public class SearchGroups : ControllerBase
{
    [HttpGet]
    [HttpPost]
    public async Task<ObjectResult> SearchGroupByName(string nome_gruppo)
    {
        //get content from url
        var content = await Utils.HtmlUtil.DownloadHtmlAsync("https://raw.githubusercontent.com/PoliNetworkOrg/polinetworkWebsiteData/main/groups.json");

        if (content == null)
        {
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" }) {StatusCode = (int) HttpStatusCode.InternalServerError};
        }

        var doc = new HtmlDocument();
      
        var c = content.GetData();
        if (c == null)
        {
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" }) {StatusCode = (int) HttpStatusCode.InternalServerError};
        }
        {
            var c1 = c.Replace("<", "&lt;");
            doc.LoadHtml(c1); 
        }
        
        //WriteLine doc
        //Console.WriteLine(doc.DocumentNode.InnerHtml);  //tenere non cancellare


        //convert doc to json
        var json = JsonConvert.DeserializeObject<dynamic>(doc.DocumentNode.InnerHtml);

        //print json file
        //return Ok(json);

        //crea  json vuoto
        var results = new JObject();
        
        //crea lista results dentro json
        
        var resultsList = new JArray();
        

        

        //cicla json
        if (json == null)
        {
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" }) {StatusCode = (int) HttpStatusCode.InternalServerError};
        }
        {
            foreach (var item in json.index_data)
            {
                //se il nome del gruppo è uguale a quello passato come parametro
                if (item["class"].ToString().ToLower().Contains(nome_gruppo.ToLower()))
                {
                    //aggiungi risultato alla lista
                    resultsList.Add(JObject.Parse(HttpUtility.HtmlDecode(item.ToString())));
                }
            } 
        }
        
        results["groups"] = resultsList;
        
        //se la lista è vuota
        if (results.Count == 0)
        {
            return new ObjectResult(new { error = "Nessun gruppo trovato" }) {
                StatusCode = (int) HttpStatusCode.InternalServerError
            };
        }
        //se la lista contiene almeno un elemento
        else
        {
            return Ok(results);
        }
    }


}

