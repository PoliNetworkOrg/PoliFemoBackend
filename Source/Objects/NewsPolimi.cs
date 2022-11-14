using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects;

public class NewsPolimi
{
    private List<string>? _content; //list of html objects (as strings)
    private readonly bool _internalNews;
    private readonly string? _subtitle;
    private readonly string? _title;
    private readonly string? _url;

    public NewsPolimi(bool internalNews, string url, string title, string subtitle)
    {
        this._internalNews = internalNews;
        this._url = url;
        this._title = title;
        this._subtitle = subtitle;
    }

    public JObject ToArticle()
    {
        var contentAsJArray = new JArray();
        if (this._content != null)
            foreach (var contentItem in this._content)
            {
                contentAsJArray.Add(contentItem);
            }

        var jObject = new JObject
        {
            ["title"] = _title,
            ["subtitle"] = _subtitle,
            ["content"] = contentAsJArray,
            ["url"] = _url,
            ["internalNews"] = this._internalNews
        };
        return jObject;
    }

    public string? GetUrl()
    {
        return this._url;
    }

    public void SetContent(List<string> list)
    {
        this._content = list;
    }
}