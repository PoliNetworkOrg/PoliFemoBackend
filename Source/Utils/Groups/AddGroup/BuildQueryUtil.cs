using PoliFemoBackend.Source.Objects.Groups;

namespace PoliFemoBackend.Source.Utils.Groups.AddGroup;

public static class BuildQueryUtil
{
    internal static string BuildQuery(Group group, Dictionary<string, object?> d)
    {
        var query = "INSERT INTO Groups VALUES ( @name, ";

        //office
        if (group.office != null)
        {
            query += "@office,";
            d.Add("@office", group.office);
        }
        else
        {
            query += "null, ";
        }

        //id
        if (!string.IsNullOrEmpty(group.id))
        {
            query += "@id,";
            d.Add("@id", group.id);
        }

        //degree
        if (group.degree != null)
        {
            query += "@degree,";
            d.Add("@degree", group.degree);
        }
        else
        {
            query += "null, ";
        }

        //school
        if (group.school != null)
        {
            query += "@school, ";
            d.Add("@school", group.school);
        }
        else
        {
            query += "null, ";
        }

        //id_link
        if (!string.IsNullOrEmpty(group.link_id))
        {
            query += "@id_link,";
            d.Add("@id_link", group.link_id);
        }


        //language
        if (!string.IsNullOrEmpty(group.language))
        {
            query += "@language,";
            d.Add("@language", group.language);
        }

        //type
        if (group.type != null)
        {
            query += "@type,";
            d.Add("@type", group.type);
        }
        else
        {
            query += "null, ";
        }

        //year
        if (group.year != null)
        {
            query += "@year, ";
            d.Add("@year", group.year);
        }
        else
        {
            query += "null, ";
        }

        //platform
        if (group.platform != null)
        {
            query += "@platform,";
            d.Add("@platform", group.platform);
        }
        else
        {
            query += "null, ";
        }

        //Last update date
        query += "now(), ";

        //Link Funzionante
        query += "'Y');";
        return query;
    }
}