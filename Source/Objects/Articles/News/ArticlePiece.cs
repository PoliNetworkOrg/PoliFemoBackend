using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticlePiece
{
    private string? _innerText;
    private readonly ArticlePieceEnum _articlePieceEnum;
    private readonly ImageDb? _imageDb;

    public ArticlePiece(ArticlePieceEnum articlePieceEnum, string argInnerHtml)
    {
        this._articlePieceEnum = articlePieceEnum;
        this._innerText = argInnerHtml;
    }

    public ArticlePiece(ArticlePieceEnum articlePieceEnum, ImageDb imageDb)
    {
        this._articlePieceEnum = articlePieceEnum;
        this._imageDb = imageDb;
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
        return this._articlePieceEnum switch
        {
            ArticlePieceEnum.TEXT => string.IsNullOrEmpty(this._innerText),
            ArticlePieceEnum.IMG => this._imageDb == null || string.IsNullOrEmpty(this._imageDb.Src),
            ArticlePieceEnum.IFRAME => string.IsNullOrEmpty(this._innerText),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public JToken ToJson()
    {
        var jObject = new JObject
        {
            ["type"] = _articlePieceEnum.ToString(),
            ["value"] = _articlePieceEnum switch
            {
                ArticlePieceEnum.TEXT => _innerText,
                ArticlePieceEnum.IMG => _imageDb?.ToJson(),
                ArticlePieceEnum.IFRAME => _innerText,
                _ => throw new ArgumentOutOfRangeException()
            }
        };
        return jObject;
    }
}