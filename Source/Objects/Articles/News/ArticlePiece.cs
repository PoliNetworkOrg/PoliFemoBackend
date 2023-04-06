using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticlePiece
{
    private string? _innerText;
    private readonly ArticlePieceEnum _articlePieceEnum;
    private readonly ImageDb? _imageDb;
    private readonly string? _htmlTag;

    private ArticlePiece(ArticlePieceEnum articlePieceEnum, string argInnerHtml, string htmlTag)
    {
        this._articlePieceEnum = articlePieceEnum;
        this._innerText = argInnerHtml;
        this._htmlTag = htmlTag;
    }

    private ArticlePiece(ArticlePieceEnum articlePieceEnum, ImageDb imageDb)
    {
        this._articlePieceEnum = articlePieceEnum;
        this._imageDb = imageDb;
    }

    private ArticlePiece(ArticlePieceEnum articlePieceEnum)
    {
        this._articlePieceEnum = articlePieceEnum;
    }

    public void FixContent()
    {
        if (string.IsNullOrEmpty(_innerText)) return;
        _innerText = _innerText.Replace("\n", "<br>");
        _innerText = _innerText.Replace("<br>", "<br />");
        _innerText = _innerText.Replace("<br /><br />", "<br />");
    }

    public bool IsEmpty() =>
        _articlePieceEnum switch
        {
            ArticlePieceEnum.TEXT => string.IsNullOrEmpty(this._innerText),
            ArticlePieceEnum.IMG => this._imageDb == null || string.IsNullOrEmpty(this._imageDb.Src),
            ArticlePieceEnum.IFRAME => string.IsNullOrEmpty(this._innerText),
            ArticlePieceEnum.LINK => this._imageDb == null || string.IsNullOrEmpty(this._imageDb.Src),
            ArticlePieceEnum.LINE => false,
            _ => throw new ArgumentOutOfRangeException()
        };

    public JToken ToJson() =>
        new JObject
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

    private JToken TextFormat() =>
        new JObject
        {
            ["c"] = this._innerText,
            ["h"] = this._htmlTag
        };

    internal static ArticlePiece? Selector(HtmlNode x)
    {
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
                return new ArticlePiece(Enums.ArticlePieceEnum.LINE);
            case "br":
                return new ArticlePiece(Enums.ArticlePieceEnum.TEXT, "\n", x.Name);
            case "figure":
            case "img":
                var a1 = new ImageDb(x.Attributes["src"].Value, x.Attributes["alt"].Value);
                return new ArticlePiece(Enums.ArticlePieceEnum.IMG,a1);
            case "#comment":
                return null;
            case "a":
                var argInnerHtml = new ImageDb(x.Attributes["src"].Value, x.Attributes["alt"].Value);
                return new ArticlePiece(ArticlePieceEnum.LINK, argInnerHtml);
            case "iframe":
                return new ArticlePiece(Enums.ArticlePieceEnum.IFRAME, x.Attributes["src"].Value, x.Name);
            default:
                Console.WriteLine(x.Name);
                break;
                    
        }
        return new ArticlePiece(Enums.ArticlePieceEnum.TEXT, x.InnerHtml, x.Name);
    }
    
    public static bool Predicate(ArticlePiece? x)
    {
        return x != null && !x.IsEmpty();
    }
}