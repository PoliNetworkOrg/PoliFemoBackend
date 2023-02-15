namespace PoliFemoBackend.Source.Objects.Articles;

public class Article
{
    public Article(string title, string content, int author_id, string tag_id, string? subtitle, string? image,
        DateTime? target_time, double? latitude, double? longitude)
    {
        this.title = title;
        this.content = content;
        this.author_id = author_id;
        this.tag_id = tag_id;
        this.subtitle = subtitle;
        this.image = image;
        this.target_time = target_time;
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public string title { get; set; }
    public string content { get; set; }
    public int author_id { get; set; }
    public string tag_id { get; set; }
    public string? subtitle { get; set; }
    public string? image { get; set; }
    public DateTime? target_time { get; set; }
    public double? latitude { get; set; }
    public double? longitude { get; set; }
}