using HtmlAgilityPack;
using Jsonize;
using Jsonize.Parser;
using Jsonize.Serializer;
using Newtonsoft.Json;
using PoliFemoBackend.Source.Utils.Article;

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
    private string? _content; //json representation of the content

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

    public void SetContent(string? text)
    {
        _content = text;
    }

    public string? GetContent()
    {
        return _content;
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


    public bool IsContentEmpty() => string.IsNullOrEmpty(_content);

    public void SetContent()
    {
        var web = new HtmlWeb();
        var doc = web.Load(_url);
        var urls1 = doc.DocumentNode.SelectNodes("//div");
        _content = HtmlToJsonUtil.GetContentFromHtml(urls1);
    }


}