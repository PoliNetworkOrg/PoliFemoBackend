using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Articles.News;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class NewsDbUtil
{
    private const int PoliMiAuthorId = 1;

    internal static DoneEnum UpdateDbWithNews(NewsPolimi newsItem)
    {
        var url = newsItem.GetUrl();
        if (string.IsNullOrEmpty(url))
            return DoneEnum.ERROR;

        const string query = "SELECT COUNT(*) FROM Articles WHERE source_url = @url";
        var args = new Dictionary<string, object?> { { "@url", url } };
        var results = Database.Database.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return DoneEnum.SKIPPED;

        var result = Database.Database.GetFirstValueFromDataTable(results);
        if (result == null)
            return DoneEnum.SKIPPED;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return DoneEnum.SKIPPED; //news already in db

        InsertItemInDb(newsItem);
        return DoneEnum.DONE;
    }

    private static void InsertItemInDb(NewsPolimi newsItem) //11111
    {
        const string query1 = "INSERT IGNORE INTO Articles " +
                              "(title,subtitle,content,publish_time,source_url,author_id,image,tag_id) " +
                              "VALUES " +
                              "(@title,@subtitle,@text_,@publishTime,@sourceUrl, @author_id, @image, @tag)";
        var args1 = new Dictionary<string, object?>
        {
            { "@sourceUrl", newsItem.GetUrl() },
            { "@title", newsItem.GetTitle() },
            { "@subtitle", newsItem.GetSubtitle() },
            { "@text_", newsItem.GetContentAsTextJson() },
            { "@publishTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@author_id", PoliMiAuthorId },
            { "@image", newsItem.GetImgUrl() },
            { "@tag", newsItem.GetTag()?.ToUpper() == "" ? "ALTRO" : newsItem.GetTag()?.ToUpper() }
        };
        Database.Database.Execute(query1, GlobalVariables.GetDbConfig(), args1);
    }
}