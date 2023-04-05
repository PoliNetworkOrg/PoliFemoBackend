using PoliFemoBackend.Source.Enums;

namespace PoliFemoBackend.Source.Objects.Articles.News;

public class ArticlePiece
{
    private string? _innerText;
    private readonly ArticlePieceEnum _articlePieceEnum;

    public ArticlePiece(ArticlePieceEnum articlePieceEnum, string argInnerHtml)
    {
        this._articlePieceEnum = articlePieceEnum;
        this._innerText = argInnerHtml;
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
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}