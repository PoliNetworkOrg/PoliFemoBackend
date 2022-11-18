namespace PoliFemoBackend.Source.Objects.Article;

public class NewsPolimi
{
    private List<string>? _content; //list of html objects (as strings)
    private readonly bool _internalNews;
    private readonly string? _subtitle;
    private readonly string? _title;
    private readonly string? _url;
    private readonly string? _tag;
    private readonly string? _imgUrl;

    public NewsPolimi(bool internalNews, string url, string title, string subtitle, string tag, string imgUrl)
    {
        _internalNews = internalNews;
        _url = url;
        _title = title;
        _subtitle = subtitle;
        _tag = tag;
        _imgUrl = imgUrl;
    }

  

    public string? GetUrl()
    {
        return _url;
    }

    public void SetContent(List<string> list)
    {
        _content = list;
    }

    public string? GetTitle()
    {
        return _title;
    }

    public string? GetSubtitle()
    {
        return _subtitle;
    }

    public string? GetTag()
    {
        return _tag;
    }

    public string? GetImgUrl()
    {
        return _imgUrl;
    }

    public string? GetContentAsTextJson()
    {
        if (_content == null)
            return null;
        
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(_content);
        return json.Trim();
    }

    public bool IsContentEmpty()
    {   
        return _content == null || _content.Count == 0 || _content.All(string.IsNullOrEmpty);
    }
}