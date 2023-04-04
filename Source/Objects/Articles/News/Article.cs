namespace PoliFemoBackend.Source.Objects.Articles.News;

public class Article
{
    public Article(string title, string content, int author_id, string tag_id, string? subtitle, string? image,
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
    }

    public string title { get; set; }
    public string content { get; set; }
    public int author_id { get; set; }
    public string tag_id { get; set; }
    public string? subtitle { get; set; }
    public string? image { get; set; }
    public DateTime? target_time { get; set; }
    public DateTime? hidden_until { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
    public string? blurhash { get; set; }
    public int platforms { get; set; }
}