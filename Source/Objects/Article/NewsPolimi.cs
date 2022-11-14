using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Article;

public class NewsPolimi
{
    private readonly bool _internalNews;
    private readonly string? _subtitle;
    private readonly string? _title;
    private readonly string? _url;
    private List<string>? _content; //list of html objects (as strings)

    public NewsPolimi(bool internalNews, string url, string title, string subtitle)
    {
        _internalNews = internalNews;
        _url = url;
        _title = title;
        _subtitle = subtitle;
    }

    public JObject ToArticle()
    {
        var contentAsJArray = new JArray();
        if (_content != null)
            foreach (var contentItem in _content)
                contentAsJArray.Add(contentItem);

        var jObject = new JObject
        {
            ["title"] = _title,
            ["subtitle"] = _subtitle,
            ["content"] = contentAsJArray,
            ["url"] = _url,
            ["internalNews"] = _internalNews
        };
        return jObject;
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

    public string? GetContentAsTextJson()
    {
        if (_content == null)
            return null;

        var json = JsonConvert.SerializeObject(_content);
        return json;
    }
}