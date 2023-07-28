#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils.Article;
using PoliNetwork.Core.Data;
using ReverseMarkdown;

#endregion

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticleContent
{
    private static readonly Config config = new()
    {
        RemoveComments = true
    };

    private static readonly Converter converter = new(config);


    public ArticleContent(string title, string? subtitle, string content)
    {
        this.title = title;
        this.subtitle = subtitle;
        this.content = content;
    }

    public ArticleContent()
    {
    }

    public string? title { get; set; }
    public string? subtitle { get; set; }
    public string? content { get; set; }
    public string? url { get; set; }

    public static ArticleContent[] LoadContentFromURL(string url)
    {
        var r = new ArticleContent[2];
        r[0] = new ArticleContent();
        r[1] = new ArticleContent();
        if (url.Contains("landingpages"))
            return r; // If this is a landing page, return an empty array as this is not a news article
        var web = new HtmlWeb();

        for (var i = 0; i < 2; i++)
        {
            var doc = web.Load(url);
            var urls1 = doc.DocumentNode.SelectNodes("//div");
            try
            {
                // Try to get the news-single-item class, if none are found, try to get the content id
                var urls = urls1.FirstOrDefault(x => x.GetAttributeValue("class", "") == "news-single-item");

                // If empty, try to get the "content" id
                if (urls == null)
                {
                    GlobalVariables.DefaultLogger.Info(
                        "No news-single-item class found, trying to get the content element");
                    urls = urls1.First(x => x.GetAttributeValue("id", "") == "content");
                }

                r[i].title = urls.SelectSingleNode("//h1").InnerHtml.Trim();
                r[i].subtitle = urls.SelectSingleNode("//h2").InnerHtml.Trim();


                var content = converter.Convert(ArticleContentUtil.CleanContentString(urls.InnerHtml));
                content = content.Replace("](/",
                    "](https://www.polimi.it/"); // Replace relative PoliMi links with absolute ones
                content = content.Replace("\r\n\r\n* * *", ""); // Remove the final horizontal line
                r[i].content = content.Trim();
                r[i].url = url;

                if (i == 0)
                {
                    var pathnode = doc.DocumentNode
                        .Descendants("li")
                        .FirstOrDefault(li => li.GetAttributeValue("id", "") == "lienglish")
                        ?.Descendants("span").First().Descendants("a").FirstOrDefault();
                    if (pathnode == null) break;
                    //var b = a.SelectSingleNode("//span").InnerHtml;
                    url = "https://polimi.it" + pathnode.GetAttributeValue("href", "");
                }
            }
            catch (Exception)
            {
                r[i].title = null;
                r[i].subtitle = null;
                r[i].content = null;
                r[i].url = null;
            }
        }

        return r;
    }
}