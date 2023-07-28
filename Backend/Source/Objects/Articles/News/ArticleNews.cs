using ReverseMarkdown;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticleNews
{
    //internal News
    private static readonly Config config = new()
    {
        RemoveComments = true
    };

    private static Converter converter = new(config);

    public ArticleContent[] content = new ArticleContent[2];

    public ArticleNews(string? tag, string? image)
    {
        this.tag = tag;
        this.image = image;
        internalNews = true;
        author_id = 1;
        content = new ArticleContent[2];
    }

    public ArticleNews(int author_id, string? image, DateTime target_time, DateTime? hidden_until, double? latitude,
        double? longitude, string? blurhash, int platforms, bool? internalNews, string? tag, ArticleContent[] content)
    {
        this.author_id = author_id;
        this.image = image;
        this.target_time = target_time;
        this.hidden_until = hidden_until;
        this.latitude = latitude;
        this.longitude = longitude;
        this.blurhash = blurhash;
        this.platforms = platforms;
        this.internalNews = internalNews;
        this.tag = tag;
        this.content = content;
    }

    public ArticleNews()
    {
    }

    public int author_id { get; set; }
    public string? image { get; set; }
    public DateTime? target_time { get; set; }
    public DateTime? hidden_until { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public string? blurhash { get; set; }
    public int? platforms { get; set; }
    public bool? internalNews { get; set; }
    public string? tag { get; set; }

    public void AddContent(ArticleContent c)
    {
        if (content[0] == null)
            content[0] = c;
        else
            content[1] = c;
    }

    public bool ShouldBeSkipped()
    {
        if (content[0] == null) return true;
        if (content[0].url == null) return true;
        return false;
    }
}