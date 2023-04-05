using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PoliFemoBackend.Source.Objects.Articles.News;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class NewsPolimi
{
    private readonly string? _imgUrl;
    private readonly bool _internalNews;
    private readonly string? _subtitle;
    private readonly string? _tag;
    private readonly string? _title;
    private readonly string? _url;
    private List<ArticlePiece>? _content; //list of html objects (as strings)

    public NewsPolimi()
    {
    }

    public NewsPolimi(bool internalNews, string url, string title, string subtitle, string? tag, string? imgUrl)
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

    public void SetContent(List<ArticlePiece>? list)
    {
        if (list != null)
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

    public JArray? GetContentAsTextJson()
    {
        if (_content == null)
            return null;

        var result = new JArray();
        foreach (var variable in _content)
        {
            result.Add(variable.ToJson());
        }
        return result;
    }

    public bool IsContentEmpty()
    {
        return _content == null || _content.Count == 0 || _content.All(x => x.IsEmpty());
    }

    public List<ArticlePiece>? GetContentAsList()
    {
        return _content;
    }

    public void FixContent()
    {
        if (_content == null)
            return;

        foreach (var x in _content)
        {
            x.FixContent();
        }
    }
}