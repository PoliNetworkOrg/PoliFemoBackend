using System.Data;
using System.Web;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Utils.Database;

namespace PoliFemoBackend.Source.Utils.Temp.Migrate;

public static class ArticleContentUpgrade
{
    public static void ArticleContentUpgradeMethod()
    {
        DataTable? x = null;
        try
        {
            x = Utils.Database.Database.ExecuteSelect("SELECT id_article, content FROM Articles", DbConfig.DbConfigVar);
        }
        catch
        {
            // ignored
        }

        if (x == null || x.Rows.Count == 0)
            return;


        foreach (DataRow dr in x.Rows)
        {
            FixArticleContent(dr["id_article"], dr["content"]);
        }
    }

    private static void FixArticleContent(object idArticle, object content)
    {
        try
        {
            var newContent = FixContent(content.ToString());
            Utils.Database.Database.Execute("UPDATE Articles SET content = @content WHERE id_article = @id_article",
                DbConfig.DbConfigVar, new Dictionary<string, object?>
                {
                    { "@content", newContent },
                    { "@id_article", idArticle }
                });
        }
        catch
        {
            // ignored
        }
    }

    private static string? FixContent(string? toString)
    {
        if (toString == null)
            return null;

        var x = (JArray?)Newtonsoft.Json.JsonConvert.DeserializeObject(toString);
        if (x == null)
            return null;

        var res = new JArray();
        foreach (var y in x)
        {
            AddValueFix(res, y);
        }

        var result = Newtonsoft.Json.JsonConvert.SerializeObject(res);
        return result;
    }

    private static void AddValueFix(JArray res, JToken y)
    {
        if (y is not JValue z)
            return;

        var done = false;
        try
        {
            var w = FixSingleItem(z);
            res.Add(w);
            done = true;
        }
        catch
        {
            // ignored
        }

        if (!done)
        {
            res.Add(z);
        }
    }

    private static string? FixSingleItem(JValue jValue)
    {
        var value = jValue.Value;
        if (value == null)
            return null;

        var s = value.ToString();

        var r = HttpUtility.UrlDecode(s);
        return r;
    }
}