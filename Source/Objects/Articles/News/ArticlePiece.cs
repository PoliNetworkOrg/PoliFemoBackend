using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticlePiece
{
    private readonly ArticlePieceEnum _articlePieceEnum;
    private readonly string? _htmlTag;
    private readonly ImageDb? _imageDb;
    private string? _innerText;

    private ArticlePiece(ArticlePieceEnum articlePieceEnum, string argInnerHtml, string htmlTag)
    {
        _articlePieceEnum = articlePieceEnum;
        _innerText = argInnerHtml;
        _htmlTag = htmlTag;
    }

    private ArticlePiece(ArticlePieceEnum articlePieceEnum, ImageDb imageDb)
    {
        _articlePieceEnum = articlePieceEnum;
        _imageDb = imageDb;
    }

    private ArticlePiece(ArticlePieceEnum articlePieceEnum)
    {
        _articlePieceEnum = articlePieceEnum;
    }

    public void FixContent()
    {
        if (string.IsNullOrEmpty(_innerText)) return;
        _innerText = _innerText.Replace("\n", "<br>");
        _innerText = _innerText.Replace("<br>", "<br />");
        _innerText = _innerText.Replace("<br /><br />", "<br />");
    }

    public bool IsEmpty()
    {
        return _articlePieceEnum switch
        {
            ArticlePieceEnum.TEXT => string.IsNullOrEmpty(_innerText),
            ArticlePieceEnum.IMG => _imageDb == null || string.IsNullOrEmpty(_imageDb.Src),
            ArticlePieceEnum.IFRAME => string.IsNullOrEmpty(_innerText),
            ArticlePieceEnum.LINK => _imageDb == null || string.IsNullOrEmpty(_imageDb.Src),
            ArticlePieceEnum.LINE => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public JToken ToJson()
    {
        return new JObject
        {
            ["type"] = _articlePieceEnum.ToString(),
            ["value"] = _articlePieceEnum switch
            {
                ArticlePieceEnum.TEXT => TextFormat(),
                ArticlePieceEnum.IMG => _imageDb?.ToJson(),
                ArticlePieceEnum.IFRAME => _innerText,
                ArticlePieceEnum.LINK => _imageDb?.ToJson(),
                ArticlePieceEnum.LINE => null,
                _ => throw new ArgumentOutOfRangeException()
            }
        };
    }

    private JToken TextFormat()
    {
        return new JObject
        {
            ["c"] = _innerText,
            ["h"] = _htmlTag
        };
    }

    internal static ArticlePiece? Selector(HtmlNode x)
    {
        try
        {
            var htmlAttributeCollection = x.Attributes;
            switch (x.Name)
            {
                case "sup":
                case "sub":
                case "em":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "strong":
                case "li":
                case "header":
                case "#text":
                case "blockquote":
                    return new ArticlePiece(ArticlePieceEnum.TEXT, x.InnerHtml, x.Name);
                case "hr":
                    return new ArticlePiece(ArticlePieceEnum.LINE);
                case "br":
                    return new ArticlePiece(ArticlePieceEnum.TEXT, "\n", x.Name);
                case "figure":
                case "img":
                    var a1 = new ImageDb(htmlAttributeCollection["src"].Value, htmlAttributeCollection["alt"].Value);
                    return new ArticlePiece(ArticlePieceEnum.IMG, a1);
                case "#comment":
                    return null;
                case "a":
                    var value = htmlAttributeCollection.Contains("src") ?  htmlAttributeCollection["src"].Value : null;
                    var alt = htmlAttributeCollection.Contains("alt") ? htmlAttributeCollection["alt"].Value : null;
                    var argInnerHtml = new ImageDb( value, alt);
                    return new ArticlePiece(ArticlePieceEnum.LINK, argInnerHtml);
                case "iframe":
                    return new ArticlePiece(ArticlePieceEnum.IFRAME, htmlAttributeCollection["src"].Value, x.Name);
                default:
                    Console.WriteLine(x.Name);
                    break;
            }
        }
        catch
        {
            ;
        }

        return new ArticlePiece(ArticlePieceEnum.TEXT, x.InnerHtml, x.Name);
        
    }

    public static bool Predicate(ArticlePiece? x)
    {
        return x != null && !x.IsEmpty();
    }
}