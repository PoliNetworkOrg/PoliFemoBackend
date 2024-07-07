#region

using HtmlAgilityPack;
using PoliFemoBackend.Source.Utils.Article;
using PoliFemoBackend.Source.Utils.News.PoliMi;
using ReverseMarkdown;

#endregion

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticleContent
{
    private static readonly Config config = new() { RemoveComments = true };

    private static readonly Converter converter = new(config);

    public ArticleContent(string title, string? subtitle, string content)
    {
        this.title = title;
        this.subtitle = subtitle;
        this.content = content;
    }

    public ArticleContent() { }

    public string? title { get; set; }
    public string? subtitle { get; set; }
    public string? content { get; set; }
    public string? url { get; set; }

    public static ArticleContent[] LoadContentFromURL(string url)
    {
        var r = new ArticleContent[2];
        r[0] = new ArticleContent();
        r[1] = new ArticleContent();

        for (var i = 0; i < 2; i++)
        {
            var doc = HtmlPageUtil.LoadUrl(url);
            var article = doc.DocumentNode.SelectSingleNode("//div[@class='article']");
            try
            {
                // Get the html article text
                var htmlContent = article.SelectSingleNode("//div[@itemprop='articleBody']");

                r[i].title = article
                    .SelectSingleNode("//h1[@itemprop='headline']")
                    .InnerText.Trim();
                r[i].subtitle = article
                    .SelectSingleNode("//div[@itemprop='description']")
                    .InnerText.Trim();

                var content = converter.Convert(htmlContent.InnerHtml);

                content = content.Replace("](/", "](https://www.polimi.it/"); // Replace relative PoliMi links with absolute ones
                r[i].content = content.Trim();
                r[i].url = url;

                if (i == 0)
                {
                    //Get the second li element (the link to the english version, if available)
                    var pathnode = doc.DocumentNode.SelectNodes("//li//a").Skip(1).FirstOrDefault();
                    if (pathnode == null)
                        break;
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
