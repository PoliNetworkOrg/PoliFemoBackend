using HtmlAgilityPack;
using ReverseMarkdown;

namespace PoliFemoBackend.Source.Objects.Articles.News;


public class ArticleContent {
    public string? title { get; set; }
    public string? subtitle { get; set; }
    public string? content { get; set; }
    public string? url { get; set; }
    private static Config config = new ReverseMarkdown.Config
    {
        RemoveComments = true
    };
    private static Converter converter = new Converter(config);


    public ArticleContent(string title, string? subtitle, string content) {
        this.title = title;
        this.subtitle = subtitle;
        this.content = content;
    }

    public ArticleContent() {
    }

    public static ArticleContent[] LoadContentFromURL(string url) {
        ArticleContent[] r = new ArticleContent[2];
        r[0] = new ArticleContent();
        r[1] = new ArticleContent();
        var web = new HtmlWeb();

        for (int i=0; i<2; i++) {
            var doc = web.Load(url);
            var urls1 = doc.DocumentNode.SelectNodes("//div");
            try {
                var urls = urls1.Where(x => x.GetClasses().Contains("news-single-item")).First();

                r[i].title = urls.SelectSingleNode("//h1").InnerHtml;
                r[i].subtitle = urls.SelectSingleNode("//h2").InnerHtml;



                var content = converter.Convert(urls.InnerHtml);
                content = content.Split("* * *\r\n\r\n")[1]; // Remove title, subtitle, and publish date
                content = content.Replace("](/", "](https://www.polimi.it/"); // Replace relative PoliMi links with absolute ones
                content = content.Replace("\r\n\r\n* * *", ""); // Remove the final horizontal line
                r[i].content = content;
                r[i].url = url;

                if (i==0) {
                    var path = doc.DocumentNode
                        .Descendants("li")
                        .FirstOrDefault(li => li.GetAttributeValue("id", "") == "lienglish")
                        ?.Descendants("span")
                        .FirstOrDefault()
                        ?.Descendants("a")
                        .First()
                        .GetAttributeValue("href", "");
                    //var b = a.SelectSingleNode("//span").InnerHtml;
                    url = "https://polimi.it" + path;
                }
            } catch (Exception) {
                r[i].title = null;
                r[i].subtitle = null;
                r[i].content = null;
                r[i].url = null;
            }

        }

        return r;
    }
}