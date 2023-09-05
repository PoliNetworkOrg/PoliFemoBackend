#region

using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Article;
using DB = PoliNetwork.Db.Utils.Database;

#endregion

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class NewsUtil
{
    private const int PoliMiAuthorId = 1;

    internal static DoneEnum UpdateDb(ArticleNews newsItem)
    {
        if (newsItem.ShouldBeSkipped()) return DoneEnum.SKIPPED;
        var url = newsItem.content[0].url;
        if (string.IsNullOrEmpty(url))
            return DoneEnum.ERROR;

        const string query = "SELECT COUNT(*) FROM ArticleContent WHERE url LIKE @url";
        var args = new Dictionary<string, object?> { { "@url", url } };
        var results = DB.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return DoneEnum.SKIPPED;

        var result = DB.GetFirstValueFromDataTable(results);
        if (result == null)
            return DoneEnum.SKIPPED;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return DoneEnum.SKIPPED; //news already in db

        InsertInDb(newsItem);
        return DoneEnum.DONE;
    }

    private static void InsertInDb(ArticleNews newsItem) //11111
    {
        var contentids = new int[newsItem.content.Length];

        foreach (var content in newsItem.content)
        {
            if (content.title == null)
            {
                contentids[Array.IndexOf(newsItem.content, content)] = -1;
                continue;
            }

            const string query = "INSERT INTO ArticleContent (url, title, subtitle, content) " +
                                 "VALUES (@url, @title, @subtitle, @content) RETURNING id";
            var args = new Dictionary<string, object?>
            {
                { "@url", content.url },
                { "@title", content.title },
                { "@subtitle", content.subtitle },
                { "@content", content.content }
            };
            var rt = DB.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
            contentids[Array.IndexOf(newsItem.content, content)] = Convert.ToInt32(rt?.Rows[0][0]);
        }


        const string query1 = "INSERT IGNORE INTO Articles " +
                              "(publish_time,author_id,image,blurhash,tag_id, platforms,content_it,content_en) " +
                              "VALUES " +
                              "(@publishTime, @author_id, @image, @blurhash, @tag, @platforms, @plit, @plen) " +
                              "ON DUPLICATE KEY UPDATE article_id = LAST_INSERT_ID(article_id)";
        var args1 = new Dictionary<string, object?>
        {
            { "@publishTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@author_id", PoliMiAuthorId },
            { "@image", newsItem.image != "" ? newsItem.image : null },
            { "@blurhash", ArticleUtil.GenerateBlurhashAsync(newsItem.image).Result },
            { "@tag", newsItem.tag?.ToUpper() == "" ? "ALTRO" : newsItem.tag?.ToUpper() },
            { "@platforms", 1 },
            { "@plit", contentids[0] },
            { "@plen", contentids[1] != -1 ? contentids[1] : null }
        };
        DB.Execute(query1, GlobalVariables.GetDbConfig(), args1);
    }
}