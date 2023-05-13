using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Enums;
using PoliFemoBackend.Source.Objects.Articles.News;
using PoliFemoBackend.Source.Utils.Article;

namespace PoliFemoBackend.Source.Utils.News.PoliMi;

public static class NewsDbUtil
{
    private const int PoliMiAuthorId = 1;

    internal static DoneEnum UpdateDbWithNews(ArticleNews newsItem)
    {
        var url = newsItem.url;
        if (string.IsNullOrEmpty(url))
            return DoneEnum.ERROR;

        const string query = "SELECT COUNT(*) FROM Articles WHERE source_url = @url";
        var args = new Dictionary<string, object?> { { "@url", url } };
        var results = Database.Database.ExecuteSelect(query, GlobalVariables.GetDbConfig(), args);
        if (results == null)
            return DoneEnum.SKIPPED;

        var result = Database.Database.GetFirstValueFromDataTable(results);
        if (result == null || newsItem.IsContentEmpty())
            return DoneEnum.SKIPPED;

        var num = Convert.ToInt32(result);
        if (num > 0)
            return DoneEnum.SKIPPED; //news already in db

        InsertItemInDb(newsItem);
        return DoneEnum.DONE;
    }

    private static void InsertItemInDb(ArticleNews newsItem) //11111
    {
        const string query1 = "INSERT IGNORE INTO Articles " +
                              "(title,subtitle,content,publish_time,source_url,author_id,image,blurhash,tag_id, platforms) " +
                              "VALUES " +
                              "(@title,@subtitle,@text_,@publishTime,@sourceUrl, @author_id, @image, @blurhash, @tag, @platforms) " +
                              "ON DUPLICATE KEY UPDATE article_id = LAST_INSERT_ID(article_id)";
        var args1 = new Dictionary<string, object?>
        {
            { "@sourceUrl", newsItem.url },
            { "@title", newsItem.title },
            { "@subtitle", newsItem.subtitle},
            { "@text_", newsItem.content },
            { "@publishTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
            { "@author_id", PoliMiAuthorId },
            { "@image", newsItem.image },
            { "@blurhash", ArticleUtil.GenerateBlurhashAsync(newsItem.image).Result },
            { "@tag", newsItem.tag?.ToUpper() == "" ? "ALTRO" : newsItem.tag?.ToUpper() },
            { "@platforms", 1}
        };
        Database.Database.Execute(query1, GlobalVariables.GetDbConfig(), args1);
    }
}