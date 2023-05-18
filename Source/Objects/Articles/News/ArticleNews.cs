using HtmlAgilityPack;
using ReverseMarkdown;
using Newtonsoft.Json;
namespace PoliFemoBackend.Source.Objects.Articles.News;
[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class ArticleNews
{
    //news from DB
    public ArticleNews(string title, string content, int author_id, string tag_id, string? subtitle, string? image,
        DateTime? target_time, DateTime? hidden_until, double? latitude, double? longitude, string? blurhash, int platforms)
    {
        this.title = title;
        this.content = content;
        this.author_id = author_id;
        this.tag_id = tag_id;
        this.subtitle = subtitle;
        this.image = image;
        this.target_time = target_time;
        this.hidden_until = hidden_until;
        this.latitude = latitude;
        this.longitude = longitude;
        this.blurhash = blurhash;
        this.platforms = platforms;
        this.internalNews = false;
    }

    //news from Polimi 
    public ArticleNews(string title, string? tag, string subtitle, string? image, string url){
        this.title = title;
        this.tag = tag;
        this.subtitle = subtitle;
        this.image = image;
        this.url = url;
        this.internalNews = true;
    }

    public ArticleNews(){

    }

    public string? title { get; set; }
    public string? content { get; set; }
    public int author_id { get; set; }
    public string? tag_id { get; set; }
    public string? subtitle { get; set; }
    public string? image { get; set; }
    public DateTime? target_time { get; set; }
    public DateTime? hidden_until { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public string? blurhash { get; set; }
    public int? platforms { get; set; }
    public bool? internalNews { get; set; }
    public string? tag { get; set; }
    public string? url { get; set;}

    //internal News
    private static Config config = new ReverseMarkdown.Config
    {
        RemoveComments = true
    };

    private static Converter converter = new Converter(config);

    public bool IsContentEmpty() => string.IsNullOrEmpty(content);

    public void SetContent(){
        if(!(internalNews ?? true))
            throw new InvalidOperationException("The article isn't of internal news type");
        try
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var urls1 = doc.DocumentNode.SelectNodes("//div");
            var urls = urls1.Where(x => x.GetClasses().Contains("news-single-item"));
            content = converter.Convert(urls.First().InnerHtml);
            content = content.Split("* * *\r\n\r\n")[1]; // Remove title, subtitle, and publish date
            content = content.Replace("](/", "](https://www.polimi.it/"); // Replace relative PoliMi links with absolute ones
            content = content.Replace("\r\n\r\n* * *", ""); // Remove the final horizontal line
        }
        catch (IndexOutOfRangeException)
        {
            // The article has a strange format, not cutting anything
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}