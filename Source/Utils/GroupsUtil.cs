#region

using System.Net;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace PoliFemoBackend.Source.Utils;

public static class GroupsUtil
{
    private static dynamic? _groups;

    public static async Task<dynamic?> GetGroups()
    {
        //if groups have been already downloaded, return them.
        if (_groups != null)
            return _groups;

        //get content from url
        var content = await HtmlUtil.DownloadHtmlAsync(Constants.Constants.GroupsUrl);

        var doc = new HtmlDocument();

        var c = content.GetData();
        if (c == null)
            return new ObjectResult(new { error = "Errore durante il recupero dei gruppi" })
                { StatusCode = (int)HttpStatusCode.InternalServerError };
        {
            var c1 = c.Replace("<", "&lt;");
            doc.LoadHtml(c1);
        }

        //WriteLine doc
        //Console.WriteLine(doc.DocumentNode.InnerHtml);  //tenere non cancellare


        //convert doc to json
        var json = JsonConvert.DeserializeObject<dynamic>(doc.DocumentNode.InnerHtml);

        if (json == null)
            return ErrorInRetrievingGroups();

        _groups = json;
        return json;
    }

    public static ObjectResult ErrorInRetrievingGroups()
    {
        return new ObjectResult(new
        {
            error = "Errore durante il recupero dei gruppi"
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };
    }

    public static JArray Filter(dynamic json, Func<dynamic, bool> filter)
    {

        var resultsList = new JArray();
        
        foreach (var item in json.index_data)
            if (filter.Invoke(item))
                resultsList.Add(JObject.Parse(HttpUtility.HtmlDecode(item.ToString())));

        return resultsList;

    }

    public static ObjectResult ResultSearch(ControllerBase controllerBase, dynamic filtered)
    {
        var results = new JObject
        {
            ["groups"] = filtered
        };

        //se la lista è vuota
        if (filtered.Count == 0)
            return new ObjectResult(new { error = "Nessun gruppo trovato" })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        
        return controllerBase.Ok(results);
    }
}