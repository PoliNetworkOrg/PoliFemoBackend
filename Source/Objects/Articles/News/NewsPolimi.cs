using HtmlAgilityPack;
using ReverseMarkdown;
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
    private string? _content; 

    private static Config config = new ReverseMarkdown.Config
    {
        RemoveComments = true,
        GithubFlavored = true
    };

    private static Converter converter = new Converter(config);

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
        try
        {
            var web = new HtmlWeb();
            var doc = web.Load(_url);
            var urls1 = doc.DocumentNode.SelectNodes("//div");
            var urls = urls1.Where(x => x.GetClasses().Contains("news-single-item"));
            _content = converter.Convert(urls.First().InnerHtml);
            _content = _content.Split("* * *\r\n\r\n")[1]; // Remove title, subtitle, and publish date
            _content = _content.Replace("\r\n", "<br>"); // Replace new lines with <br>
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