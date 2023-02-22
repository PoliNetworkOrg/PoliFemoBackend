﻿using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;

namespace PoliFemoBackend.Source.Utils.Groups.ModifyGroup;

public class BuildQueryUtil
{
    internal static string BuildQuery(JObject ob, string id, Dictionary<string, object?> d)
    {
        var query = "UPDATE Groups SET id=@id, ";
        //office
        if (ob["name"] != null)
        {
            query += "class = @name,";
            d.Add("@name", ob["name"]);
        }

        //office
        if (ob["office"] != null)
        {
            query += "office = @office,";
            d.Add("@office", ob["office"]);
        }

        //degree
        if (ob["degree"] != null)
        {
            query += "degree = @degree,";
            d.Add("@degree", ob["degree"]);
        }

        //school
        if (ob["school"] != null)
        {
            query += "school =@school, ";
            d.Add("@school", ob["school"]);
        }


        //id_link
        if (!string.IsNullOrEmpty(ob["link_id"]?.ToString()))
        {
            query += "link_id = @l,";
            d.Add("@l", ob["link_id"]);
        }


        //language
        if (!string.IsNullOrEmpty(ob["language"]?.ToString()))
        {
            query += "language = @language,";
            d.Add("@language", ob["language"]);
        }


        //type
        if (ob["type"] != null)
        {
            query += "type_ = @type,";
            d.Add("@type", ob["type"]);
        }

        //year
        if (ob["year"] != null)
        {
            query += "year = @year, ";
            d.Add("@year", ob["year"]);
        }

        //platform
        if (ob["platform"] != null)
        {
            query += "platform = @platform,";
            d.Add("@platform", ob["platform"]);
        }

        //Last update date
        query += "last_updated = now(), ";

        //condizione se viene aggiornato il platform , year e link_id
        if (ob["platform"] != null || ob["year"] != null || !string.IsNullOrEmpty(ob["link_id"]?.ToString()))
        {
            //fai select in cui recuperi i paramentri non modificati
            const string query2 = "SELECT * FROM Groups WHERE id = @id";
            var results = Database.Database.ExecuteSelect(query2, GlobalVariables.DbConfigVar, d);
            //salva i parametri non modificati in una variabile
            if (ob["platform"] == null)
                d.Add("@platform", results?.Rows[0]["platform"]);

            if (ob["year"] == null)
                d["@year"] = results?.Rows[0]["year"];

            if (string.IsNullOrEmpty(ob["link_id"]?.ToString()))
                d["@l"] = results?.Rows[0]["link_id"];

            //richiama generatedID per generare un nuovo id
            d["@id"] = GenerateHash.GeneratedId(d["@platform"] + "/" + d["@year"] + "/" + d["@l"]);
        }

        //Link Funzionante
        query += "link_is_working = 'Y' ";

        d.Add("@old_id", id);
        query += "WHERE id= @old_id;";
        return query;
    }
}